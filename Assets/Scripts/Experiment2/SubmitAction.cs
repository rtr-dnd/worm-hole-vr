using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;

public class SubmitAction : MonoBehaviour
{
  public GameObject CompleteMsg;
  ToggleGroup q0;
  ToggleGroup q1;
  ToggleGroup q2;
  ToggleGroup q3;

  // Start is called before the first frame update
  void Start()
  {
    q0 = this.gameObject.transform.Find("Q0").transform.Find("Group").GetComponent<ToggleGroup>();
    q1 = this.gameObject.transform.Find("Q1").transform.Find("Group").GetComponent<ToggleGroup>();
    q2 = this.gameObject.transform.Find("Q2").transform.Find("Group").GetComponent<ToggleGroup>();
    q3 = this.gameObject.transform.Find("Q3").transform.Find("Group").GetComponent<ToggleGroup>();
  }
  public void Submit()
  {
    Toggle t0 = q0.ActiveToggles().FirstOrDefault();
    Toggle t1 = q1.ActiveToggles().FirstOrDefault();
    Toggle t2 = q2.ActiveToggles().FirstOrDefault();
    Toggle t3 = q3.ActiveToggles().FirstOrDefault();

    // record answer
    var folder = Application.persistentDataPath;
    var filePath = Path.Combine(folder, SceneContextHolder.filePrefix + "_" + (SceneContextHolder.timeStamp) + ".csv");
    using (var writer = new StreamWriter(filePath, true))
    {
      // axis, condition, button, trial, q0, q1, q2, q3
      writer.Write($"{SceneContextHolder.axis},{SceneContextHolder.currentCondition},{SceneContextHolder.currentButton},{SceneContextHolder.progress[SceneContextHolder.currentCondition]},{SceneContextHolder.currentButton},{t0.name},{t1.name},{t2.name},{t3.name}\n");
      Debug.Log("written results");
    }
    SceneContextHolder.progress[SceneContextHolder.currentCondition] += 1;
    SceneContextHolder.buttonProgress[SceneContextHolder.currentCondition, SceneContextHolder.currentButton] += 1;

    if (SceneContextHolder.progress.Sum() >= SceneContextHolder.conditionNum * SceneContextHolder.trialNum)
    {
      Debug.Log("experiment complete");
      CompleteMsg.SetActive(true);
      return;
    }

    // determine condition
    bool hasDecidedCondition = false;
    int nextCondition = 0;
    int tempCondition = 0;
    while (!hasDecidedCondition)
    {
      if (tempCondition > 1000)
      {
        Debug.Log("too many loops");
        hasDecidedCondition = true;
      }
      nextCondition = UnityEngine.Random.Range(0, SceneContextHolder.conditionNum);
      Debug.Log("how about " + nextCondition);
      Debug.Log("progress is " + SceneContextHolder.progress[nextCondition]);
      if (SceneContextHolder.progress[nextCondition] < SceneContextHolder.trialNum)
      {
        hasDecidedCondition = true;
      }
      else
      {
        tempCondition++;
        continue;
      }
    }
    SceneContextHolder.currentCondition = nextCondition;

    // determine button
    bool hasDecidedButton = false;
    int nextButton = 0;
    int tempButton = 0;
    while (!hasDecidedButton)
    {
      if (tempButton > 1000)
      {
        Debug.Log("too many loops");
        hasDecidedButton = true;
      }
      nextButton = UnityEngine.Random.Range(0, 5);
      Debug.Log("how about " + nextButton);
      Debug.Log("button progress is " + SceneContextHolder.buttonProgress[nextCondition, nextButton]);
      if (SceneContextHolder.buttonProgress[nextCondition, nextButton] < SceneContextHolder.trialNum / 5)
      {
        hasDecidedButton = true;
      }
      else
      {
        tempButton++;
        continue;
      }
    }
    SceneContextHolder.currentButton = nextButton;

    Debug.Log("this is " + SceneContextHolder.progress.Sum() + "th time");

    // load next scene
    if (nextCondition < (SceneContextHolder.conditionNum / 2))
    {
      SceneManager.LoadScene("TaskWorm");
    }
    else
    {
      SceneManager.LoadScene("TaskRedirection");
    }
  }
}
