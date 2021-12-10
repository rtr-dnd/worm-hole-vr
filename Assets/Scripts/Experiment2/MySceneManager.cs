using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
  public string axis;
  public GameObject shoulderTracker;
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
    // Invoke("RecordShoulder", 1f);
    // Invoke("ChangeScene", 2f);
  }

  private void Update()
  {
    if (Input.GetKeyDown("r"))
    {
      SceneContextHolder.shoulderPosition = shoulderTracker.transform.position;
      Debug.Log("recorded shoulder position: " + SceneContextHolder.shoulderPosition);
    }
    if (Input.GetKeyDown("n"))
    {
      SceneManager.LoadScene(firstSceneName);
    }
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
  public static Vector3 shoulderPosition { get; set; }
}