using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingObject : MonoBehaviour
{
  public bool isTouching;
  private string targetObjectName;
  // Start is called before the first frame update
  void Start()
  {
    if (SceneContextHolder.isLeftHanded)
    {
      targetObjectName = "ColliderLeft";
    }
    else
    {
      targetObjectName = "ColliderRight";
    }
  }

  // Update is called once per frame
  void LateUpdate()
  {
    isTouching = false;
  }
  private void OnCollisionStay(Collision other)
  {
    if (other.gameObject.name == targetObjectName)
    {
      isTouching = true;
      Debug.Log("colliding with " + targetObjectName);
    }
  }
}
