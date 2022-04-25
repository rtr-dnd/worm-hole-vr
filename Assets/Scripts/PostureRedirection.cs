using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostureRedirection : MonoBehaviour
{
    [SerializeField] TaskManager taskManager;
    // Start is called before the first frame update
    [SerializeField] Transform redirectedManusHandLeft;
    [SerializeField] Transform redirectedManusHandRight;
    [SerializeField] Transform RealDisplacement;
    private Transform sourceTransform;
    private Transform targetTransform;
    private Transform hand;
    private GameObject handParent;
    private Transform redirectedHand;
    private float distance;
    void Start()
    {
        //Get Transform of maker (source position)
        sourceTransform = taskManager.marker.transform;
        //Get Manus real hand
        if (SceneContextHolder.isLeftHanded){
            hand = taskManager.redirectionRealHandLeft.transform;
            redirectedHand = redirectedManusHandLeft.transform;
        }else{
            hand = taskManager.redirectionRealHandRight.transform;
            redirectedHand = redirectedManusHandRight.transform;
        }
        //Get Transform of board (target positino)
        targetTransform = taskManager.RealToVirtualDisplacement.transform;

        distance = Vector3.Distance(sourceTransform.position, targetTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Copy HaRT_CoreWithVR Hand to RealDisplacement Hand!
        redirectedHand.transform.position = hand.transform.position;
        redirectedHand.transform.localScale = hand.transform.localScale;
        redirectedHand.transform.rotation = hand.transform.rotation;
        //RealToVirtualDisplacementとHandとの初期位置からのボードまでの距離とボードから手までの距離の比率
        float ratio = Vector3.Distance(targetTransform.position, redirectedHand.position) / distance;
        //RealDisplacement　と　RealToVirtualDisplacementの中間な傾きをratioの割合で算出
        var redirectQuarternion = Quaternion.Lerp(RealDisplacement.rotation, targetTransform.rotation, ratio);
        handParent.transform.localRotation = redirectQuarternion;
    }
}
