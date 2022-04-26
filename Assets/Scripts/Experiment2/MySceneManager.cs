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

  public bool isExperiment3;

  private string firstSceneName;
  // Start is called before the first frame update
  void Start()
  {
    SceneContextHolder.isExperiment3 = isExperiment3;
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
      if(isExperiment3){
        firstSceneName = "TaskWorm_3";
      }
    }
    else
    {
      firstSceneName = "TaskRedirection";
      if(isExperiment3){
        firstSceneName = "TaskRedirection_3";
      }
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
        if(isExperiment3){
          SceneManager.LoadScene("TaskWorm_3");
        }else{
          SceneManager.LoadScene("TaskWorm");
        } 
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
  public static bool isExperiment3;
  public static long timeStamp;
  public static string filePrefix;
  public static string axis { get; set; }
  public static int trialNum { get; set; }
  public static int conditionNum { get; set; }
  // conditions: [ws, wo, rs, ro]
  // conditions: [w-90, w-75, w-60, w-45, w-30, w-15, w0, w+15, w+30, w+45, w+60, w+75, w+90,
  //              r-90, r-75, r-60, r-45, r-30, r-15, r0, r+15, r+30, r+45, r+60, r+75, r+90] 26
  public static int currentCondition { get; set; }
  public static int[] progress { get; set; }
  public static int[,] buttonProgress { get; set; }
  public static int currentButton { get; set; }
  public static bool isLeftHanded { get; set; }
  public static bool isPractice { get; set; }
  public static int practiceProgress { get; set; }
}