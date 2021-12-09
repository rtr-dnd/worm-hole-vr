using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInputModule : BaseInputModule
{
  public Camera m_Camera;
  public SteamVR_Input_Sources m_TargetSource;
  public SteamVR_Action_Boolean m_ClickAction;

  private GameObject m_CurrentObject = null;
  private PointerEventData m_Data = null;
  // Start is called before the first frame update
  protected override void Awake()
  {
    base.Awake();

    m_Data = new PointerEventData(eventSystem);
  }

  // Update is called once per frame
  public override void Process()
  {
    // reset data, set camera
    m_Data.Reset();
    m_Data.position = new Vector2(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);

    // raycast
    eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
    m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
    m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;

    // clear
    m_RaycastResultCache.Clear();

    // Hover
    HandlePointerExitAndEnter(m_Data, m_CurrentObject);

    // Press
    if (m_ClickAction.GetStateDown(m_TargetSource))
      ProcessPress(m_Data);

    // Release
    if (m_ClickAction.GetStateUp(m_TargetSource))
      ProcessRelease(m_Data);

  }

  public PointerEventData GetData()
  {
    return m_Data;
  }

  private void ProcessPress(PointerEventData data)
  {

  }

  private void ProcessRelease(PointerEventData data)
  {

  }
}
