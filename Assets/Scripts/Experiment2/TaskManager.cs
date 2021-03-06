using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using HR_Toolkit;

public class TaskManager : MonoBehaviour
{
  public GameObject RealToVirtualDisplacement;
  public GameObject redirectionManager;
  public GameObject marker;
  public GameObject hitArea;
  public GameObject props;
  public GameObject rightHandReal;
  public GameObject rightHandVirtual;
  public GameObject leftHandReal;
  public GameObject leftHandVirtual;
  public GameObject head;
  public Material targetMaterial;
  public Material completeMaterial;
  public Material markerActiveMaterial;
  public Material markerInactiveMaterial;
  public GameObject redirectionVirtualHandRight;
  public GameObject redirectionVirtualHandLeft;
  public GameObject redirectionRealHandRight;
  public GameObject redirectionRealHandLeft;
  public GameObject redirectionTrackedHandRight;
  public GameObject redirectionTrackedHandLeft;
  private Renderer targetBasebaseRenderer;
  private StringBuilder sb;
  private Vector3 headPositionOnStart;
  private bool markerIsActive;

  // 0: init, 1: hand placed, 2: reaching, 3: button pressed, 4: task completed
  private int state = 0;
  private int pressedButton = -1;
  // Start is called before the first frame update
  void Start()
  {
    Debug.Log(SceneContextHolder.axis);
    Debug.Log(SceneContextHolder.currentCondition);
    Debug.Log(SceneContextHolder.progress);
    sb = new StringBuilder("time, state, pressedButton, markerIsActive, pxRightReal, pyRightReal, pzRightReal, rxRightReal, ryRightReal, rzRightReal, eulerxRightReal, euleryRightReal, eulerzRightReal, pxRightVirtual, pyRightVirtual, pzRightVirtual, rxRightVirtual, ryRightVirtual, rzRightVirtual, eulerxRightVirtual, euleryRightVirtual, eulerzRightVirtual, pxLeftReal, pyLeftReal, pzLeftReal, rxLeftReal, ryLeftReal, rzLeftReal, eulerxLeftReal, euleryLeftReal, eulerzLeftReal, pxLeftVirtual, pyLeftVirtual, pzLeftVirtual, rxLeftVirtual, ryLeftVirtual, rzLeftVirtual, eulerxLeftVirtual, euleryLeftVirtual, eulerzLeftVirtual, pxHead, pyHead, pzHead, rxHead, ryHead, rzHead, eulerxHead, euleryHead, eulerzHead\n");

    if (redirectionManager != null)
    {
      RedirectionManager rm = redirectionManager.GetComponent<RedirectionManager>();
      MovementController mc = redirectionManager.GetComponent<MovementController>();
      // if (SceneContextHolder.isPractice)
      // {
      //   rm.initialTargetIndex = 4;
      // }
      // else
      // {
      rm.initialTargetIndex = SceneContextHolder.currentButton;
      // }
      if (SceneContextHolder.isLeftHanded)
      {
        rm.virtualHand = redirectionVirtualHandLeft;
        mc.trackedHand = redirectionTrackedHandLeft;
      }
      else
      {
        rm.virtualHand = redirectionVirtualHandRight;
        mc.trackedHand = redirectionTrackedHandRight;
      }
    }

    Invoke("initializeHead", 0.5f);
  }

  // Update is called once per frame
  void Update()
  {
    markerIsActive = hitArea.GetComponent<CollidingObject>().isTouching;
    marker.GetComponent<Renderer>().material = markerIsActive
        ? markerActiveMaterial
        : markerInactiveMaterial;

    switch (state)
    {
      case 0:
        if (markerIsActive)
        {
          Debug.Log("state 0 => 1: ready, set");
          state = 1;
        }
        break;
      case 1:
        if (!markerIsActive)
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
        if (markerIsActive)
        {
          Debug.Log("state 3 => 4: marker touched");
          state = 4;
        }
        break;
      case 4:
        Invoke("nextScene", 1f);
        break;
    }

    if (Input.GetKeyDown("r"))
    {
      state = 0;
      headPositionOnStart = head.transform.position;
      targetBasebaseRenderer.material = targetMaterial;
    }
    else if (Input.GetKeyDown("n"))
    {
      if (SceneContextHolder.isPractice)
      {
        SceneContextHolder.practiceProgress += 1;
        switch (SceneContextHolder.practiceProgress)
        {
          case 1:
            SceneManager.LoadScene("TaskRedirection");
            break;
          case 2:
            SceneManager.LoadScene("Evaluation");
            break;
          // 3: evalutation => redirection 0
          default:
            SceneManager.LoadScene("TaskRedirection");
            break;
        }
      }
    }
    else if (Input.GetKeyDown("0"))
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
      .Append(state).Append(", ")
      .Append(pressedButton).Append(", ")
      .Append(markerIsActive).Append(", ")
      .Append(logOf(rightHandReal)).Append(", ")
      .Append(logOf(rightHandVirtual)).Append(", ")
      .Append(logOf(leftHandReal)).Append(", ")
      .Append(logOf(leftHandVirtual)).Append(", ")
      .Append(logOf(head));
  }

  string logOf(GameObject obj)
  {
    return (
        obj.transform.position.x + ", "
        + obj.transform.position.y + ", "
        + obj.transform.position.z + ", "
        + obj.transform.rotation.x + ", "
        + obj.transform.rotation.y + ", "
        + obj.transform.rotation.z + ", "
        + obj.transform.eulerAngles.x + ", "
        + obj.transform.eulerAngles.y + ", "
        + obj.transform.eulerAngles.z
        );
  }

  void nextScene()
  {
    if (SceneContextHolder.isPractice) return;

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

  void initializeHead()
  {
    headPositionOnStart = head.transform.position;
    Debug.Log("head set at " + headPositionOnStart);

    initializeDisplacement();
    initializeProps();
  }
  void initializeDisplacement()
  {
    // set origin to head position
    Vector3 headDisplace = headPositionOnStart - RealToVirtualDisplacement.transform.position;
    RealToVirtualDisplacement.transform.position = headPositionOnStart;
    // undo offset for children
    for (int a = 0; a < RealToVirtualDisplacement.transform.childCount; a++)
    {
      RealToVirtualDisplacement.transform.GetChild(a).position -= headDisplace;
    }
    RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(0, 0, 0);

    if (SceneContextHolder.isPractice && SceneContextHolder.practiceProgress >= 3)
    {
      return;
    }
    else if (SceneContextHolder.currentCondition % 2 == 0)
    {
      // subtle
      switch (SceneContextHolder.axis)
      {
        case "r":
          Debug.Log("r subtle");
          RealToVirtualDisplacement.transform.position += new Vector3(0, 0, 0.2f);
          break;
        case "v":
          Debug.Log("v subtle");
          RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(-20, 0, 0);
          break;
        case "h":
          Debug.Log("h subtle");
          if (SceneContextHolder.isLeftHanded)
          {
            RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(0, -20, 0);
          }
          else
          {
            RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(0, 20, 0);
          }
          break;
        default:
          RealToVirtualDisplacement.transform.position += new Vector3(0, 0, 0.2f);
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
        case "h":
          Debug.Log("h overt");
          if (SceneContextHolder.isLeftHanded)
          {
            RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(0, -45, 0);
          }
          else
          {
            RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(0, 45, 0);
          }
          break;
        default:
          RealToVirtualDisplacement.transform.position += new Vector3(0, 0, 0.2f);
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
