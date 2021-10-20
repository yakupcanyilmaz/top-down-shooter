using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
  public LayerMask whatIsRoom;
  public GameManager gameManager;

  void Update()
  {
    Collider[] roomDetection = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, whatIsRoom);
    if (roomDetection.Length == 0 && gameManager.stopGeneration == true)
    {
      int rand = Random.Range(0, gameManager.rooms.Length);
      Instantiate(gameManager.rooms[rand], transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }
}
