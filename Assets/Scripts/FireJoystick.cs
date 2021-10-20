using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FireJoystick : Joystick
{
  private bool touched;
  private int pointerID;
  private bool canFire;

  protected override void Start()
  {
    base.Start();
    // background.gameObject.SetActive(false);
    touched = false;
  }

  public override void OnPointerDown(PointerEventData eventData)
  {
    background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
    background.gameObject.SetActive(true);
    if (!touched)
    {
      touched = true;
      pointerID = eventData.pointerId;
      canFire = true;
    }
    base.OnPointerDown(eventData);
  }

  public override void OnPointerUp(PointerEventData eventData)
  {
    background.gameObject.SetActive(false);
    if (eventData.pointerId == pointerID)
    {
      canFire = false;
      touched = false;
    }
    base.OnPointerUp(eventData);
  }

  public bool CanFire()
  {
    return canFire;
  }

}