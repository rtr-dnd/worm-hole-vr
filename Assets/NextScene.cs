using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
  public string triggerKey;
  public string nextSceneName;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(triggerKey))
    {
      SceneManager.LoadScene(nextSceneName);
    }
  }
}
