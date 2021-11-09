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
  // Start is called before the first frame update
  void Start()
  {
    sb = new StringBuilder("time, px, py, pz, rx, ry, rz, collision");
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void FixedUpdate()
  {
    sb.Append('\n')
      .Append(Time.fixedTimeAsDouble).Append(", ")
      .Append(motionTarget.transform.position.x).Append(", ")
      .Append(motionTarget.transform.position.y).Append(", ")
      .Append(motionTarget.transform.position.z).Append(", ")
      .Append(motionTarget.transform.rotation.x).Append(", ")
      .Append(motionTarget.transform.rotation.y).Append(", ")
      .Append(motionTarget.transform.rotation.z).Append(", ")
      .Append(triggerFlag);
    triggerFlag = false;
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
