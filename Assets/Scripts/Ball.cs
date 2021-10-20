using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
  public float speed = 10f;
  private int damage;

  Vector3 direction;

  public enum Balls
  {
    BIG,
    MEDIUM,
    SMALL,
  }
  public Balls ball;
  public GameObject ballToSpawn;
  public GameObject explosion;

  void Start()
  {
    GetComponent<Rigidbody>().velocity = Random.insideUnitSphere.normalized * speed;
  }

  void DestroyBall()
  {
    if (ball == Balls.BIG)
    {
      for (int i = 0; i < 2; i++)
      {
        Vector3 rot = new Vector3(Random.Range(0, 360), 0, Random.Range(0, 360));
        GameObject newBall = Instantiate(ballToSpawn, transform.position, Quaternion.Euler(rot)) as GameObject;
        Ball ballScript = newBall.GetComponent<Ball>();
        ballScript.ball = Balls.MEDIUM;
      }

      Instantiate(explosion, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }

    if (ball == Balls.MEDIUM)
    {
      for (int i = 0; i < 2; i++)
      {
        Vector3 rot = new Vector3(Random.Range(0, 360), 0, Random.Range(0, 360));
        GameObject newBall = Instantiate(ballToSpawn, transform.position, Quaternion.Euler(rot)) as GameObject;
        Ball ballScript = newBall.GetComponent<Ball>();
        ballScript.ball = Balls.SMALL;
      }

      Instantiate(explosion, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }

    if (ball == Balls.SMALL)
    {
      Instantiate(explosion, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Bullet")
    {
      other.gameObject.SetActive(false);
      DestroyBall();
    }
  }

  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.tag == "Player")
    {
      if (ball == Balls.BIG)
      {
        GetComponent<Ball>().damage = 3;
      }
      if (ball == Balls.MEDIUM)
      {
        GetComponent<Ball>().damage = 2;
      }
      if (ball == Balls.SMALL)
      {
        GetComponent<Ball>().damage = 1;
      }
      FindObjectOfType<PlayerHealth>().TakeDamage(damage);
      Instantiate(explosion, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
    if (other.gameObject.tag == "Shield")
    {
      Instantiate(explosion, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }
}
