using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArm : MonoBehaviour
{
    public GameObject elbowOrigin;

    private Transform hand;
    private Transform elbow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hand = this.gameObject.transform.parent;
        elbow = elbowOrigin.transform;
        Vector3 temp = new Vector3(
            0.05f,
            Vector3.Distance(hand.position, elbow.position) / 2,
            0.05f
        );
        this.gameObject.transform.localScale = temp;
        this.gameObject.transform.position = (hand.position + elbow.position) / 2;
        this.gameObject.transform.LookAt(elbow);
        this.gameObject.transform.Rotate(new Vector3(90, 0, 0));
    }
}
