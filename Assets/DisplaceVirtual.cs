using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class DisplaceVirtual : MonoBehaviour
{
    public GameObject RealCounterpart;
    public GameObject RealHole;
    public GameObject VirtualHole;
    private Matrix4x4 oMt; // origin to tracker
    private Matrix4x4 tMc; // tracker to chair
    private Matrix4x4 chairMat;
    private SteamVR_Action_Pose tracker1 = SteamVR_Actions.default_Pose;

    // Start is called before the first frame update    

    void Start()
    {
        tMc = Matrix4x4.TRS(
            RealCounterpart.transform.localPosition,
            RealCounterpart.transform.localRotation,
            RealCounterpart.transform.localScale
        );
    }

    // Update is called once per frame
    void Update()
    {
        oMt = Matrix4x4.TRS(
            tracker1.GetLocalPosition(SteamVR_Input_Sources.RightHand),
            tracker1.GetLocalRotation(SteamVR_Input_Sources.RightHand),
            new Vector3(1, 1, 1)
        );

        chairMat = 
        Matrix4x4.Translate(VirtualHole.transform.position - RealHole.transform.position) // real to virtual translation
        * Matrix4x4.TRS(
            RealHole.transform.position,
            Quaternion.Euler(new Vector3(0f, 90f, 0f)),
            Vector3.one
        ) // translation back to real hole and rotation around real hole
        * Matrix4x4.Translate(-RealHole.transform.position) // offset translation for next step
        * oMt * tMc; // real chair

        this.transform.position = chairMat.GetColumn(3);
        this.transform.rotation = chairMat.rotation;
        this.transform.localScale = chairMat.lossyScale;
    }
}
