using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChildrenWithKey : MonoBehaviour
{
  // Start is called before the first frame update
  public string showKey;
  public string hideKey;
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(showKey))
    {
      for (int a = 0; a < this.transform.childCount; a++)
      {
        this.transform.GetChild(a).gameObject.SetActive(true);
      }
    }
    if (Input.GetKeyDown(hideKey))
    {
      for (int a = 0; a < this.transform.childCount; a++)
      {
        this.transform.GetChild(a).gameObject.SetActive(false);
      }
    }
  }
}
