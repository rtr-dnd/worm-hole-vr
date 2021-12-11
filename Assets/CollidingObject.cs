using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingObject : MonoBehaviour
{
  public bool isTouching;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void LateUpdate()
  {
    isTouching = false;
  }
  private void OnCollisionStay(Collision other)
  {
    if (other.gameObject.name == "ColliderRight")
    {
      isTouching = true;
      Debug.Log("colliding with right hand");
    }
  }
}
