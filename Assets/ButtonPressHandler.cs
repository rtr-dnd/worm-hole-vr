using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressHandler : MonoBehaviour
{
  public string key;
  public Material onMaterial;
  public Material offMaterial;
  private Transform top;
  // Start is called before the first frame update
  void Start()
  {
    top = gameObject.transform.Find("top");
    top.GetComponent<Renderer>().material = offMaterial;
    top.GetComponent<Animator>().SetBool("isPressed", false);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(key))
    {
      top.GetComponent<Renderer>().material = onMaterial;
      top.GetComponent<Animator>().SetBool("isPressed", true);
    }
    else
    {
      top.GetComponent<Renderer>().material = offMaterial;
      top.GetComponent<Animator>().SetBool("isPressed", false);
    }
  }
}
