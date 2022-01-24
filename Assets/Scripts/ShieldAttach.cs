using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShieldAttach : MonoBehaviour
{
    // https://forum.unity.com/threads/xr-interaction-toolkit-1-0-0-pre-2-pre-release-is-available.1046092/

    public GameObject otherHand;

    void Start() {
        XRSocketInteractor socket = gameObject.GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(Disable);
        socket.selectExited.AddListener(Enable);
    }

    public void Disable(SelectEnterEventArgs args) {
        XRSocketInteractor otherSocket = otherHand.GetComponent<XRSocketInteractor>();
        otherSocket.enabled = false;
    }

    public void Enable(SelectExitEventArgs args) {
        XRSocketInteractor otherSocket = otherHand.GetComponent<XRSocketInteractor>();
        otherSocket.enabled = true;
    }
    
}
