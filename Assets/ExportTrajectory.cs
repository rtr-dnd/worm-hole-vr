using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ExportTrajectory : MonoBehaviour
{
  public string filePrefix;
  public GameObject motionTarget;

  private StringBuilder sb;
  private bool triggerFlag;
  private bool isLogging;
  // Start is called before the first frame update
  void Start()
  {
    sb = new StringBuilder("time, px, py, pz, rx, ry, rz, eulerx, eulery, eulerz, collision");
    isLogging = false;
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void FixedUpdate()
  {

    if (Input.GetKeyDown("b"))
    {
      isLogging = true;
    }
    else if (Input.GetKeyDown("e"))
    {
      isLogging = false;
      sb.Append('\n')
        .Append("----------");
    }

    if (isLogging)
    {
      sb.Append('\n')
        .Append(Time.fixedTimeAsDouble).Append(", ")
        .Append(motionTarget.transform.position.x).Append(", ")
        .Append(motionTarget.transform.position.y).Append(", ")
        .Append(motionTarget.transform.position.z).Append(", ")
        .Append(motionTarget.transform.rotation.x).Append(", ")
        .Append(motionTarget.transform.rotation.y).Append(", ")
        .Append(motionTarget.transform.rotation.z).Append(", ")
        .Append(motionTarget.transform.eulerAngles.x).Append(", ")
        .Append(motionTarget.transform.eulerAngles.y).Append(", ")
        .Append(motionTarget.transform.eulerAngles.z).Append(", ")
        .Append(triggerFlag);
      triggerFlag = false;
    }
  }

  private void OnDestroy()
  {
    var folder = Application.persistentDataPath;
    System.DateTime centuryBegin = new System.DateTime(2001, 1, 1);
    var filePath = Path.Combine(folder, filePrefix + (System.DateTime.Now.Ticks - centuryBegin.Ticks) + ".csv");
    using (var writer = new StreamWriter(filePath, false))
    {
      writer.Write(sb.ToString());
    }
  }

  private void OnTriggerStay(Collider other)
  {
    triggerFlag = true;
    Debug.Log("Trigger enter: " + other.gameObject.name);
  }
}
