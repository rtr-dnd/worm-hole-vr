using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
  public string axis;
  public int trialNum;
  public int conditionNum;
  public GameObject shoulderTracker;
  private string firstSceneName;
  // Start is called before the first frame update
  void Start()
  {
    System.DateTime centuryBegin = new System.DateTime(2001, 1, 1);
    SceneContextHolder.timeStamp = System.DateTime.Now.Ticks - centuryBegin.Ticks;
    SceneContextHolder.axis = axis;
    SceneContextHolder.trialNum = trialNum;
    SceneContextHolder.conditionNum = conditionNum;

    SceneContextHolder.currentCondition = UnityEngine.Random.Range(0, conditionNum);
    if (SceneContextHolder.currentCondition < (conditionNum / 2)) // first half is worm, latter half is redirection
    {
      firstSceneName = "TaskWorm";
    }
    else
    {
      firstSceneName = "TaskRedirection";
    }
    SceneContextHolder.progress = new int[conditionNum];
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
  public static long timeStamp;
  public static string axis { get; set; }
  public static int trialNum { get; set; }
  public static int conditionNum { get; set; }
  // conditions: [ws, wo, rs, ro]
  public static int currentCondition { get; set; }
  public static int[] progress { get; set; }
  public static Vector3 shoulderPosition { get; set; }
}