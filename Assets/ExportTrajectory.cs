using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ExportTrajectory : MonoBehaviour
{
  public string filePrefix;
  public GameObject target;

  private StringBuilder sb;
  // Start is called before the first frame update
  void Start()
  {
    sb = new StringBuilder("time, px, py, pz, rx, ry, rz");
  }

  // Update is called once per frame
  void Update()
  {

  }

  private void FixedUpdate()
  {
    sb.Append('\n')
      .Append(Time.fixedTimeAsDouble).Append(", ")
      .Append(target.transform.position.x).Append(", ")
      .Append(target.transform.position.y).Append(", ")
      .Append(target.transform.position.z).Append(", ")
      .Append(target.transform.rotation.x).Append(", ")
      .Append(target.transform.rotation.y).Append(", ")
      .Append(target.transform.rotation.z);
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
}
