using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
  [Header("Control Settings:")]
  public FireJoystick fireJoystick;


  [Header("Statistics:")]
  public float timeBetweenShots = .2f;

  [Header("References:")]
  public GameObject turret;
  public Transform shotPoint;
  public GameObject crosshair;
  // public GameObject bulletPrefab;
  public GameObject sightLine;
  public CameraShake cameraShake;
  public AudioSource shootingAudio;
  public AudioClip fireClip;


  private float aimXValue;
  private float aimZValue;
  private Animator cameraAnim;
  private Camera cam;
  private Vector3 cursorPoint;
  [HideInInspector]
  public bool canFire;

  void Start()
  {
    crosshair.transform.position = transform.position;
    cam = Camera.main;
  }

  private void Update()
  {
    ProcessInputs();
    Aim();
    Fire();
  }

  private void ProcessInputs()
  {
    if (!GameManager.useMobileController)
    {
      sightLine.SetActive(false);
      cursorPoint = cam.ScreenToWorldPoint(Input.mousePosition);
      crosshair.transform.position = new Vector3(cursorPoint.x, crosshair.transform.position.y, cursorPoint.z);
      fireJoystick.gameObject.SetActive(false);
    }

    if (GameManager.useMobileController)
    {
      crosshair.SetActive(false);
      aimXValue = fireJoystick.Horizontal;
      aimZValue = fireJoystick.Vertical;
    }
  }

  private void Aim()
  {
    if (!GameManager.useMobileController)
    {
      Vector3 aimDir = crosshair.transform.localPosition;
      aimDir.y = 0f;
      turret.transform.rotation = Quaternion.LookRotation(aimDir);
    }

    if (GameManager.useMobileController)
    {

      if (aimXValue != 0 || aimZValue != 0)
      {
        Vector3 aimDir = new Vector3(aimXValue, 0, aimZValue).normalized;
        turret.transform.rotation = Quaternion.LookRotation(aimDir);
        sightLine.SetActive(true);
      }
      else
      {
        sightLine.SetActive(false);
      }
    }
  }

  private void Fire()
  {
    if (!GameManager.useMobileController)
    {
      if (!canFire && Input.GetButton("Fire1"))
      {
        canFire = true;
        // Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
        // GameObject bullet = Pool.singleton.Get("Bullet");
        PrefabManager.PlayPrefab(PrefabType.Bullet, shotPoint.position, shotPoint.rotation);
        // if (bullet != null)
        // {
        //   bullet.transform.position = shotPoint.position;
        //   bullet.transform.rotation = shotPoint.rotation;
        //   bullet.SetActive(true);
        // }
        shootingAudio.clip = fireClip;
        shootingAudio.Play();
        StartCoroutine(ShootDelay());
        StartCoroutine(cameraShake.Shake(.1f, .1f));
      }
    }

    if (GameManager.useMobileController)
    {
      if (!canFire && fireJoystick.CanFire())
      {
        canFire = true;
        // Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);
        // GameObject bullet = Pool.singleton.Get("Bullet");
        PrefabManager.PlayPrefab(PrefabType.Bullet, shotPoint.position, shotPoint.rotation);
        // if (bullet != null)
        // {
        //   bullet.transform.position = shotPoint.position;
        //   bullet.transform.rotation = shotPoint.rotation;
        //   bullet.SetActive(true);
        // }
        shootingAudio.clip = fireClip;
        shootingAudio.Play();
        StartCoroutine(ShootDelay());
        StartCoroutine(cameraShake.Shake(.1f, .1f));
      }
    }
  }

  IEnumerator ShootDelay()
  {
    yield return new WaitForSeconds(timeBetweenShots);
    canFire = false;
  }

}
