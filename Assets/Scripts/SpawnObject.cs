﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
  public GameObject[] objects;

  private void Start()
  {
    int rand = Random.Range(0, objects.Length);
    GameObject instance = (GameObject)Instantiate(objects[rand], transform.position, transform.rotation);
    instance.transform.parent = transform;
  }
}
