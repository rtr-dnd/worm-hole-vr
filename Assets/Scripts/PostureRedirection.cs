using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostureRedirection : MonoBehaviour
{
    [SerializeField] TaskManager taskManager;
    // Start is called before the first frame update
    [SerializeField] GameObject ghostManusHandLeft;
    [SerializeField] GameObject ghostManusHandRight;
    private Transform sourceTransform;
    private Transform targetTransform;
    private Transform hand;

    private Transform ghostHand;
    private Quaternion initRealitveRoation;

    private float distance;
    void Start()
    {
        //Get Transform of maker (source position)
        sourceTransform = taskManager.marker.transform;
        //Get Manus real hand
        if (SceneContextHolder.isLeftHanded){
            hand = taskManager.redirectionRealHandLeft.transform;
            ghostHand = ghostManusHandLeft.transform;
        }else{
            hand = taskManager.redirectionRealHandRight.transform;
            ghostHand = ghostManusHandRight.transform;
        }
        //Get Transform of board (target positino)
        targetTransform = taskManager.RealToVirtualDisplacement.transform;
    }

    // Update is called once per frame
    void Update()
    {
        ghostHand.transform.position = hand.transform.position;
    }
}
