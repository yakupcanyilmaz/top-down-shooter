using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

  public Transform player;

  void LateUpdate()
  {
    if (player != null)
    {
      transform.position = new Vector3(player.position.x, -0.4f, player.position.z);
    }
    else
    {
      gameObject.SetActive(false);
    }
  }
}
