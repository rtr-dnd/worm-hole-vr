using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class SubmitAction : MonoBehaviour
{
  public GameObject CompleteMsg;
  ToggleGroup q1;
  ToggleGroup q2;

  // Start is called before the first frame update
  void Start()
  {
    q1 = this.gameObject.transform.Find("Q1").transform.Find("Group").GetComponent<ToggleGroup>();
    q2 = this.gameObject.transform.Find("Q2").transform.Find("Group").GetComponent<ToggleGroup>();
  }

  // Update is called once per frame
  public void Submit()
  {
    Toggle t1 = q1.ActiveToggles().FirstOrDefault();
    Toggle t2 = q2.ActiveToggles().FirstOrDefault();
    Debug.Log("Q1: " + t1.name);
    Debug.Log("Q2: " + t2.name);

    // record answer
    // todo: write log
    SceneContextHolder.progress[SceneContextHolder.currentCondition] += 1;
    if (SceneContextHolder.progress.Sum() >= SceneContextHolder.conditionNum * SceneContextHolder.trialNum)
    {
      Debug.Log("experiment complete");
      CompleteMsg.SetActive(true);
      return;
    }

    bool hasDecidedCondition = false;
    int nextCondition = 0;
    int temp = 0;
    while (!hasDecidedCondition)
    {
      if (temp > 100)
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
        temp++;
        continue;
      }
    }
    SceneContextHolder.currentCondition = nextCondition;

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
