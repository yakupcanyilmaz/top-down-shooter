using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
  public struct PrefabInstance
  {
    public GameObject Effect;
    public int Index;
  }

  static PrefabManager Instance { get; set; }

  public PrefabDatabase Database;

  Queue<PrefabInstance>[] m_Instances;

  void Awake()
  {
    Instance = this;
    Init();
  }

  void Init()
  {
    m_Instances = new Queue<PrefabInstance>[Database.Entries.Length];
    for (int i = 0; i < Database.Entries.Length; i++)
    {
      m_Instances[i] = new Queue<PrefabInstance>();
      CreateNewInstance(i);
    }
  }

  void CreateNewInstance(int index)
  {
    var entry = Database.Entries[index];

    for (int j = 0; j < entry.PoolSize; j++)
    {
      PrefabInstance PrefabInstance = new PrefabInstance();
      var inst = Instantiate(entry.Prefab);
      inst.gameObject.SetActive(false);

      PrefabInstance.Effect = inst;
      PrefabInstance.Index = index;

      m_Instances[index].Enqueue(PrefabInstance);
    }
  }

  public static PrefabInstance GetPrefab(PrefabType type)
  {
    int idx = (int)type;
    if (Instance.m_Instances[idx].Count == 0)
    {
      Instance.CreateNewInstance(idx);
    }

    var inst = Instance.m_Instances[idx].Dequeue();
    Instance.m_Instances[idx].Enqueue(inst);

    inst.Effect.gameObject.SetActive(true);
    return inst;
  }

  public static PrefabInstance PlayPrefab(PrefabType type, Vector3 position, Quaternion rotation)
  {
    var i = GetPrefab(type);
    i.Effect.gameObject.transform.position = position;
    i.Effect.gameObject.transform.rotation = rotation;

    return i;
  }

}

public enum PrefabType
{
  Bullet
}
