using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TaskManager : MonoBehaviour
{
  public GameObject RealToVirtualDisplacement;
  public GameObject marker;
  public GameObject hitArea;
  public GameObject props;
  public GameObject rightHand;
  public Material targetMaterial;
  public Material completeMaterial;
  public Material markerActiveMaterial;
  public Material markerInactiveMaterial;
  private Renderer targetBasebaseRenderer;
  private StringBuilder sb;

  // 0: init, 1: hand placed, 2: reaching, 3: button pressed, 4: task completed
  private int state = 0;
  private int pressedButton = -1;
  // Start is called before the first frame update
  void Start()
  {
    Debug.Log(SceneContextHolder.axis);
    Debug.Log(SceneContextHolder.currentCondition);
    Debug.Log(SceneContextHolder.progress);
    sb = new StringBuilder("time, px, py, pz, rx, ry, rz, eulerx, eulery, eulerz, state, pressedButton");

    initializeDisplacement();
    initializeProps();
  }

  // Update is called once per frame
  void Update()
  {
    marker.GetComponent<Renderer>().material = hitArea.GetComponent<CollidingObject>().isTouching
        ? markerActiveMaterial
        : markerInactiveMaterial;

    switch (state)
    {
      case 0:
        if (hitArea.GetComponent<CollidingObject>().isTouching)
        {
          Debug.Log("state 0 => 1: ready, set");
          state = 1;
        }
        break;
      case 1:
        if (!hitArea.GetComponent<CollidingObject>().isTouching)
        {
          Debug.Log("state 1 => 2: go");
          state = 2;
        }
        break;
      case 2:
        if (Input.GetKeyDown(SceneContextHolder.currentButton.ToString()))
        {
          Debug.Log("state 2 => 3: button pressed");
          targetBasebaseRenderer.material = completeMaterial;
          state = 3;
        }
        break;
      case 3:
        if (hitArea.GetComponent<CollidingObject>().isTouching)
        {
          Debug.Log("state 3 => 4: button pressed");
          state = 4;
        }
        break;
      case 4:
        Invoke("nextScene", 1f);
        break;
    }

    if (Input.GetKeyDown("0"))
    {
      pressedButton = 0;
    }
    else if (Input.GetKeyDown("1"))
    {
      pressedButton = 1;
    }
    else if (Input.GetKeyDown("2"))
    {
      pressedButton = 2;
    }
    else if (Input.GetKeyDown("3"))
    {
      pressedButton = 3;
    }
    else if (Input.GetKeyDown("4"))
    {
      pressedButton = 4;
    }
    else
    {
      pressedButton = -1;
    }


    sb.Append('\n')
      .Append(Time.fixedTimeAsDouble).Append(", ")
      .Append(rightHand.transform.position.x).Append(", ")
      .Append(rightHand.transform.position.y).Append(", ")
      .Append(rightHand.transform.position.z).Append(", ")
      .Append(rightHand.transform.rotation.x).Append(", ")
      .Append(rightHand.transform.rotation.y).Append(", ")
      .Append(rightHand.transform.rotation.z).Append(", ")
      .Append(rightHand.transform.eulerAngles.x).Append(", ")
      .Append(rightHand.transform.eulerAngles.y).Append(", ")
      .Append(rightHand.transform.eulerAngles.z).Append(", ")
      .Append(state).Append(", ")
      .Append(pressedButton);
  }

  void nextScene()
  {
    var folder = Application.persistentDataPath;
    // axis, condition, trial, button
    if (SceneContextHolder.progress != null)
    {
      var filePath = Path.Combine(folder, $"{SceneContextHolder.filePrefix}_{SceneContextHolder.timeStamp}_{SceneContextHolder.axis}_{SceneContextHolder.currentCondition}_{SceneContextHolder.progress[SceneContextHolder.currentCondition]}_{SceneContextHolder.currentButton}.csv");
      using (var writer = new StreamWriter(filePath, false))
      {
        writer.Write(sb.ToString());
        Debug.Log("written results");
      }
    }
    SceneManager.LoadScene("Evaluation");
  }

  void initializeDisplacement()
  {
    // set origin to shoulder position
    Vector3 shoulderDisplace = SceneContextHolder.shoulderPosition - RealToVirtualDisplacement.transform.position;
    // RealToVirtualDisplacement.transform.position += shoulderDisplace;
    RealToVirtualDisplacement.transform.position = SceneContextHolder.shoulderPosition;
    // undo offset for children
    for (int a = 0; a < RealToVirtualDisplacement.transform.childCount; a++)
    {
      RealToVirtualDisplacement.transform.GetChild(a).position -= shoulderDisplace;
    }
    RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(0, 0, 0);

    if (SceneContextHolder.currentCondition % 2 == 0)
    {
      // subtle
      switch (SceneContextHolder.axis)
      {
        case "r":
          Debug.Log("r subtle");
          RealToVirtualDisplacement.transform.position += new Vector3(0, 0, 0.1f);
          break;
        case "v":
          Debug.Log("v subtle");
          RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(-20, 0, 0);
          break;
        default:
          RealToVirtualDisplacement.transform.position += new Vector3(0, 0, 0.1f);
          break;
      }
    }
    else
    {
      // overt
      switch (SceneContextHolder.axis)
      {
        case "r":
          Debug.Log("r overt");
          RealToVirtualDisplacement.transform.position += new Vector3(0, 0, 1f);
          break;
        case "v":
          Debug.Log("v overt");
          RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(-45, 0, 0);
          break;
        default:
          RealToVirtualDisplacement.transform.position += new Vector3(0, 0, 0.1f);
          break;
      }
    }
  }

  void initializeProps()
  {
    switch (SceneContextHolder.currentButton)
    {
      case 0:
        targetBasebaseRenderer = props.transform.Find("ButtonLower").Find("BaseBase").GetComponent<Renderer>();
        break;
      case 1:
        targetBasebaseRenderer = props.transform.Find("ButtonRight").Find("BaseBase").GetComponent<Renderer>();
        break;
      case 2:
        targetBasebaseRenderer = props.transform.Find("ButtonUpper").Find("BaseBase").GetComponent<Renderer>();
        break;
      case 3:
        targetBasebaseRenderer = props.transform.Find("ButtonLeft").Find("BaseBase").GetComponent<Renderer>();
        break;
      case 4:
        targetBasebaseRenderer = props.transform.Find("ButtonCenter").Find("BaseBase").GetComponent<Renderer>();
        break;
      default:
        break;
    }
    targetBasebaseRenderer.material = targetMaterial;
  }
}
