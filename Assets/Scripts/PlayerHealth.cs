using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
  public Image[] lifes;
  public Sprite full;
  public Sprite empty;
  public int health = 5;
  public Animator hurtAnim;
  public GameObject explosion;

  public void TakeDamage(int amount)
  {
    health -= amount;
    UpdateHealthUI(health);
    hurtAnim.SetTrigger("hurt");
    if (health <= 0)
    {
      Destroy(gameObject);
      Instantiate(explosion, transform.position, Quaternion.identity);
    }
  }

  void UpdateHealthUI(int currentHealth)
  {
    for (int i = 0; i < lifes.Length; i++)
    {
      if (i < currentHealth)
      {
        lifes[i].sprite = full;
      }
      else
      {
        lifes[i].sprite = empty;
      }
    }
  }
}
