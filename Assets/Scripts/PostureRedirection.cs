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
        if (SceneContextHolder.isLeftHanded){
            hand = taskManager.redirectionRealHandLeft.transform;
            targetHand = targetManusHandLeft.transform;
        }else{
            hand = taskManager.redirectionRealHandRight.transform;
            targetHand = targetManusHandRight.transform;
        }
        //Get Transform of board (target positino)
        targetTransform = taskManager.RealToVirtualDisplacement.transform;
        //Distance is the position difference between source and target
        distance = Vector3.Distance(sourceTransform.position, targetTransform.position);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     //Copy HaRT_CoreWithVR Hand to RealDisplacement Hand!
    //     targetHand.transform.position = hand.transform.position;
    //     targetHand.transform.localScale = hand.transform.localScale;
    //     targetHand.transform.localRotation = hand.transform.rotation;
    //     //RealToVirtualDisplacementとHandとの初期位置からのボードまでの距離とボードから手までの距離の比率
    //     float ratio = Vector3.Distance(targetTransform.position, targetHand.position) / distance;
    //     //RealDisplacement　と　RealToVirtualDisplacementの中間な傾きをratioの割合で算出
    //     var redirectQuarternion = Quaternion.Lerp(RealDisplacement.rotation, targetTransform.rotation, ratio);
    //     Debug.Log("Redirected : " + redirectQuarternion);
    //     handParent.transform.rotation = redirectQuarternion;
    // }
    
    //hand parentの傾きを0にして、そのあとでまた、変える方法
    // void Update()
    // {
    //     handParent.transform.rotation = Quaternion.Euler(0.0f, 0.0f ,0.0f);
    //     //Copy HaRT_CoreWithVR Hand to RealDisplacement Hand!
    //     targetHand.transform.rotation = hand.transform.rotation;
    //     targetHand.transform.position = hand.position;
    //     //RealToVirtualDisplacementとHandとの初期位置からのボードまでの距離とボードから手までの距離の比率
    //     float ratio = Vector3.Distance(targetTransform.position, targetHand.position) / distance;
    //     //RealDisplacement　と　RealToVirtualDisplacementの中間な傾きをratioの割合で算出
    //     var redirectQuarternion = Quaternion.Lerp(RealDisplacement.rotation, targetTransform.rotation, ratio);
    //     Debug.Log("Redirected : " + redirectQuarternion);
    //     handParent.transform.rotation = redirectQuarternion;
    // }
    // void Update()
    // {
    //     var handCopy = GameObject.Instantiate(hand);
    //     //Copy HaRT_CoreWithVR Hand to RealDisplacement Hand!
    //     handCopy.position = new Vector3(0.0f, 0.0f, 0.0f);
    //     targetHand.transform.rotation = handCopy.rotation;
    //     targetHand.transform.localPosition = hand.position;
    //     //RealToVirtualDisplacementとHandとの初期位置からのボードまでの距離とボードから手までの距離の比率
    //     float ratio = Vector3.Distance(targetTransform.position, targetHand.position) / distance;
    //     //RealDisplacement　と　RealToVirtualDisplacementの中間な傾きをratioの割合で算出
    //     var redirectQuarternion = Quaternion.Lerp(RealDisplacement.rotation, targetTransform.rotation, ratio);
    //     Debug.Log("Redirected : " + redirectQuarternion);
    //     handParent.transform.rotation = redirectQuarternion;
    //     GameObject.Destroy(hand);
    // }
    void Update()
    {
        //Copy the real hand to virtual hand
        targetHand.transform.position = hand.transform.position;
        // targetHand.transform.rotation = hand.transform.rotation;
        targetHand.transform.localScale = hand.transform.localScale;
        float ratio = Vector3.Distance(targetTransform.position, targetHand.position) / distance;
        //RealDisplacement　と　RealToVirtualDisplacementの中間な傾きをratioの割合で算出
        var redirectQuarternion = Quaternion.Lerp(sourceTransform.rotation, targetTransform.rotation, ratio);
        Debug.Log("Redirected : " + redirectQuarternion);
        targetHand.rotation = redirectQuarternion * Quaternion.Inverse(sourceTransform.rotation) * hand.rotation;
    }
}
