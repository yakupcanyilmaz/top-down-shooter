using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform target;
  public float speed;
  // public float minX;
  // public float maxX;
  // public float minZ;
  // public float maxZ;

  private Vector3 moveVelocity;

  private void FixedUpdate()
  {
    if (target != null)
    {
      // float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
      // float clampedZ = Mathf.Clamp(target.position.z, minZ, maxZ);

      Vector3 trg = new Vector3(target.transform.position.x, target.transform.position.y + 10, target.transform.position.z);
      transform.position = Vector3.SmoothDamp(transform.position, trg, ref moveVelocity, 0.2f);

      // transform.position = Vector3.Lerp(transform.position, new Vector3(clampedX, 0, clampedZ), speed);
    }
  }
}
