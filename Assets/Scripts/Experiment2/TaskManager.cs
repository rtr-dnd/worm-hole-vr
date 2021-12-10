using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    Debug.Log(SceneContextHolder.axis);
    Debug.Log(SceneContextHolder.currentCondition);
    Debug.Log(SceneContextHolder.progress);
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
