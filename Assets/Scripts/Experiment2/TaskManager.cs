using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
  public GameObject RealToVirtualDisplacement;
  public GameObject marker;
  public GameObject hitArea;
  public GameObject props;
  public Material targetMaterial;
  public Material completeMaterial;
  public Material markerActiveMaterial;
  public Material markerInactiveMaterial;
  private Renderer targetBasebaseRenderer;

  // 0: init, 1: hand placed, 2: reaching, 3: button pressed, 4: task completed
  private int state = 0;
  // Start is called before the first frame update
  void Start()
  {
    Debug.Log(SceneContextHolder.axis);
    Debug.Log(SceneContextHolder.currentCondition);
    Debug.Log(SceneContextHolder.progress);

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
  }

  void nextScene()
  {
    SceneManager.LoadScene("Evaluation");
  }

  void initializeDisplacement()
  {
    // set origin to shoulder position
    RealToVirtualDisplacement.transform.position = SceneContextHolder.shoulderPosition;
    // undo offset for children
    for (int a = 0; a < RealToVirtualDisplacement.transform.childCount; a++)
    {
      RealToVirtualDisplacement.transform.GetChild(a).position -= SceneContextHolder.shoulderPosition;
    }

    if (SceneContextHolder.currentCondition % 2 == 0)
    {
      // subtle
      switch (SceneContextHolder.axis)
      {
        case "r":
          Debug.Log("r subtle");
          RealToVirtualDisplacement.transform.position = new Vector3(0, 0, 0.1f);
          RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(0, 0, 0);
          break;
        case "v":
          Debug.Log("v subtle");
          RealToVirtualDisplacement.transform.position = new Vector3(0, 0, 0);
          RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(-20, 0, 0);
          break;
        default:
          RealToVirtualDisplacement.transform.position = new Vector3(0, 0, 0);
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
          RealToVirtualDisplacement.transform.position = new Vector3(0, 0, 1f);
          RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(0, 0, 0);
          break;
        case "v":
          Debug.Log("v overt");
          RealToVirtualDisplacement.transform.position = new Vector3(0, 0, 0);
          RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(-45, 0, 0);
          break;
        default:
          RealToVirtualDisplacement.transform.position = new Vector3(0, 0, 0);
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
