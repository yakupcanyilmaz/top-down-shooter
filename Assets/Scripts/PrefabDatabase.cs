using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabDatabase", menuName = "Database/PrefabDatabase")]
public class PrefabDatabase : ScriptableObject
{
  [System.Serializable]
  public struct PrefabDBEntry
  {
    public string Name;
    public GameObject Prefab;
    public int PoolSize;
  }

  public PrefabDBEntry[] Entries;
}
