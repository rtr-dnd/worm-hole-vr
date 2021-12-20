using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
  public string filePrefix;
  public string axis;
  public int trialNum;
  public int conditionNum;
  public bool isPractice;
  public bool isLeftHanded;
  public GameObject shoulderTracker;
  private string firstSceneName;
  // Start is called before the first frame update
  void Start()
  {
    SceneContextHolder.isPractice = isPractice;
    SceneContextHolder.isLeftHanded = isLeftHanded;
    if (isPractice) { SceneContextHolder.currentButton = 4; }
    if (isPractice) return;

    System.DateTime centuryBegin = new System.DateTime(2001, 1, 1);
    SceneContextHolder.timeStamp = System.DateTime.Now.Ticks - centuryBegin.Ticks;
    SceneContextHolder.filePrefix = filePrefix;
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
    SceneContextHolder.buttonProgress = new int[conditionNum, 5];
    SceneContextHolder.currentButton = UnityEngine.Random.Range(0, 5);
    // Invoke("RecordShoulder", 1f);
    // Invoke("ChangeScene", 2f);
  }

  private void Update()
  {
    if (Input.GetKeyDown("n"))
    {
      if (isPractice)
      {
        SceneManager.LoadScene("TaskWorm");
        return;
      }
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
  public static string filePrefix;
  public static string axis { get; set; }
  public static int trialNum { get; set; }
  public static int conditionNum { get; set; }
  // conditions: [ws, wo, rs, ro]
  public static int currentCondition { get; set; }
  public static int[] progress { get; set; }
  public static int[,] buttonProgress { get; set; }
  public static int currentButton { get; set; }
  public static bool isLeftHanded { get; set; }
  public static bool isPractice { get; set; }
  public static int practiceProgress { get; set; }
}