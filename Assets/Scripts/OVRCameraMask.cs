using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRCameraMask : MonoBehaviour
{
    private Camera ovrCam;

    void Start()
    {
        ovrCam = gameObject.GetComponent<OVRCameraRig>().leftEyeCamera; // means center eye camera when per eye cam false

        int layer1 = LayerMask.NameToLayer("ClearedPuzzleMarker");
        int layer2 = LayerMask.NameToLayer("VisitedMarker");

        ovrCam.cullingMask = ~(1 << layer1 | 1 << layer2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
