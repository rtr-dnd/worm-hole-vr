using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubmitAction : MonoBehaviour
{
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
  }
}
