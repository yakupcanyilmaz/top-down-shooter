using UnityEngine;
using UnityEngine.UI;
// using System.Collections;
// using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
  [Header("Control Settings:")]
  public MoveJoystick moveJoystick;

  [Header("Statistics:")]
  public float speed = 5f;
  public float turnSpeed = 5f;

  private Vector3 direction;
  private Rigidbody rb;
  private float movementInputValue;
  private float turnInputValue;

  private void Awake()
  {
    rb = GetComponent<Rigidbody>();
  }

  private void OnEnable()
  {
    rb.isKinematic = false;
    movementInputValue = 0f;
    turnInputValue = 0f;
  }

  private void OnDisable()
  {
    rb.isKinematic = true;
  }

  private void FixedUpdate()
  {
    ProcessInputs();
    Move();
    Turn();
  }

  private void ProcessInputs()
  {
    if (!GameManager.useMobileController)
    {
      movementInputValue = Input.GetAxis("Vertical");
      turnInputValue = Input.GetAxis("Horizontal");
      direction = new Vector3(turnInputValue, 0, movementInputValue).normalized;
      moveJoystick.gameObject.SetActive(false);
    }

    if (GameManager.useMobileController)
    {
      movementInputValue = moveJoystick.Vertical;
      turnInputValue = moveJoystick.Horizontal;
      direction = new Vector3(turnInputValue, 0, movementInputValue).normalized;
    }
  }

  private void Move()
  {
    Vector3 movement = direction * speed * Time.deltaTime;
    rb.MovePosition(transform.position + movement);
    rb.velocity = Vector3.zero;
  }

  private void Turn()
  {
    if (movementInputValue != 0 || turnInputValue != 0)
    {
      Quaternion turnRotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
      rb.MoveRotation(Quaternion.Slerp(transform.rotation, turnRotation, Time.deltaTime * turnSpeed));
    }
  }
}
