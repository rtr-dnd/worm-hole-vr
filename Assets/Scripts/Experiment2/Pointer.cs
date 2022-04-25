using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

  public float m_defaultLength = 5.0f;
  public GameObject mDot;
  public VRInputModule m_inputModule;
  private LineRenderer m_LineRenderer = null;
  // Start is called before the first frame update
  private void Awake()
  {
    m_LineRenderer = GetComponent<LineRenderer>();
  }


  // Update is called once per frame
  private void Update()
  {
    UpdateLine();
  }

  private void UpdateLine()
  {
    // use default or distance
    float targetLength = m_defaultLength;

    // Raycast
    RaycastHit hit = CreateRaycast(targetLength);

    // Default
    Vector3 endPosition = transform.position + (transform.forward * targetLength);

    // or based on hit
    if (hit.collider != null)
    {
      endPosition = hit.point;
    }

    // set position of the dot
    m_LineRenderer.SetPosition(0, transform.position);
    m_LineRenderer.SetPosition(1, endPosition);
  }

  private RaycastHit CreateRaycast(float length)
  {
    RaycastHit hit;
    Ray ray = new Ray(transform.position, transform.forward);
    Physics.Raycast(ray, out hit, m_defaultLength);

    return hit;
  }

}
