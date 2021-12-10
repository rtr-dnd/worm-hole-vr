using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
  public GameObject RealToVirtualDisplacement;
  // Start is called before the first frame update
  void Start()
  {
    Debug.Log(SceneContextHolder.axis);
    Debug.Log(SceneContextHolder.currentCondition);
    Debug.Log(SceneContextHolder.progress);

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
          break;
        case "v":
          Debug.Log("v subtle");
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
          break;
        case "v":
          Debug.Log("v overt");
          RealToVirtualDisplacement.transform.localEulerAngles = new Vector3(-45, 0, 0);
          break;
        default:
          RealToVirtualDisplacement.transform.position = new Vector3(0, 0, 0);
          break;
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown("n"))
    {
      SceneManager.LoadScene("Evaluation");
    }
  }
}
