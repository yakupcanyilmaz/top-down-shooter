using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
  [Header("Statistics:")]
  public float speed = 15f;
  public float lifeTime = 1f;
  public int damage = 1;

  [Header("References:")]
  public GameObject muzzlePrefab;
  public GameObject hitPrefab;

  private Rigidbody rb;

  private void Start()
  {
    rb = GetComponent<Rigidbody>();

    Invoke("DestroyBullet", lifeTime);

    if (muzzlePrefab != null)
    {
      var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
      muzzleVFX.transform.forward = gameObject.transform.forward;
      var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
      if (psMuzzle != null)
      {
        Destroy(muzzleVFX, psMuzzle.main.duration);
      }
      else
      {
        var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(muzzleVFX, psChild.main.duration);
      }
    }
  }

  private void FixedUpdate()
  {
    rb.velocity = transform.forward * speed;
  }

  private void DestroyBullet()
  {
    InstantiateHitVFX();
    Destroy(gameObject);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      other.GetComponent<PlayerHealth>().TakeDamage(damage);
      DestroyBullet();
    }
    else if (other.tag == "Wall")
    {
      DestroyBullet();
    }
  }

  private void InstantiateHitVFX()
  {
    if (hitPrefab != null)
    {
      var hitVFX = Instantiate(hitPrefab, transform.position, Quaternion.identity);
      hitVFX.transform.forward = gameObject.transform.forward;
      var pshit = hitVFX.GetComponent<ParticleSystem>();
      if (pshit != null)
      {
        Destroy(hitVFX, pshit.main.duration);
      }
      else
      {
        var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(hitVFX, psChild.main.duration);
      }
    }
  }
}
