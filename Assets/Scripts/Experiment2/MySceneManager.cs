using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
  public string axis;
  private string firstSceneName;
  // Start is called before the first frame update
  void Start()
  {
    SceneContextHolder.axis = axis;
    SceneContextHolder.currentCondition = UnityEngine.Random.Range(0, 3);
    if (SceneContextHolder.currentCondition < 2)
    {
      firstSceneName = "TaskWorm";
    }
    else
    {
      firstSceneName = "TaskRedirection";
    }
    SceneContextHolder.progress = new int[4];
    Invoke("ChangeScene", 1.5f);
  }

  void ChangeScene()
  {

    SceneManager.LoadScene(firstSceneName);
  }
}

public static class SceneContextHolder
{
  public static string axis { get; set; }
  // conditions: [ws, wo, rs, ro]
  public static int currentCondition { get; set; }
  public static int[] progress { get; set; }
}