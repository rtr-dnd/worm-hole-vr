using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostureRedirection : MonoBehaviour
{
  [SerializeField] TaskManager taskManager;
  // Start is called before the first frame update
  [SerializeField] Transform targetManusHandLeft;
  [SerializeField] Transform targetManusHandRight;
  [SerializeField] Transform RealDisplacement;
  // [SerializeField] Transform handParent;

  [SerializeField] Transform RealManusHandLeft;

  [SerializeField] Transform RealManusHandRight;

  [SerializeField] Transform RealToVirtualDisplacement;
  [SerializeField] Transform Props;
  private Transform sourceTransform;
  private Transform targetTransform;
  private Transform hand;
  private Transform targetHand;
  private float distance;
  void Start()
  {
    //Get Transform of maker (source position)
    sourceTransform = taskManager.marker.transform;
    //Get Manus real hand
    if (SceneContextHolder.isLeftHanded)
    {
      hand = RealManusHandLeft.transform;
      targetHand = targetManusHandLeft.transform;
    }
    else
    {
      hand = RealManusHandRight.transform;
      targetHand = targetManusHandRight.transform;
    }
    //Get Transform of board (target positino)
    // targetTransform = RealToVirtualDisplacement;
    distance = Vector3.Distance(sourceTransform.position, Props.position);
    Debug.Log("Distance : " + distance);
    Debug.Log("source : " + sourceTransform.position + " target : " + targetTransform.position);
    Debug.Log("source : " + sourceTransform.rotation.eulerAngles + " target : " + targetTransform.rotation.eulerAngles);


    Debug.Log(hand + ": " + targetHand);
  }

  // void Update()
  // {
  //   targetHand.position = hand.position;
  //   targetHand.localRotation = hand.localRotation;
  //   var ratio = Vector3.Distance(Props.position, hand.position) / distance;
  //   if (ratio > 1.0f)
  //   {
  //     ratio = 1.0f;
  //   }
  //   var redirectQuarternion = Quaternion.Lerp(RealToVirtualDisplacement.rotation, RealDisplacement.rotation, ratio);
  //   targetHand.rotation = redirectQuarternion * hand.rotation;
  //   Debug.Log("ratio : " + ratio + " : " + redirectQuarternion.eulerAngles + " : " + targetHand.rotation + " : " + hand.rotation);
  // }
  void Update()
  {

    var Props2Hand = hand.position - Props.position;
    targetHand.localRotation = hand.localRotation;
    var ratio = Vector3.Distance(Props.position, hand.position) / distance;
    if (ratio > 1.0f)
    {
      ratio = 1.0f;
    }
    //redirectQuarternion is the middle rotation os RealToVirtualDisplacement and Maker
    var redirectQuarternion = Quaternion.Lerp(RealToVirtualDisplacement.rotation, RealDisplacement.rotation, ratio);
    targetHand.rotation = redirectQuarternion * hand.rotation;
    targetHand.position = (redirectQuarternion * Quaternion.Inverse(RealDisplacement.rotation)) * Props2Hand + Props.position;
    Debug.Log("ratio : " + ratio + " : " + redirectQuarternion.eulerAngles + " : " + targetHand.rotation + " : " + hand.rotation);
  }
}
