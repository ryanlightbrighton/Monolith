using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Puzzle : MonoBehaviour
{
    public Material mat;
    private GameObject exit;
    private BoxCollider trigger;

    void Start() {
        exit = GameObject.FindGameObjectWithTag("Finish");
        trigger = exit.GetComponent<BoxCollider>();
        //trigger.enabled = false;
    }

    public void ButtonPressed() {
        if (CheckCubes()) {
            ChangeExitColor();
            DestroyBarriers();
            AddExit();
        }
    }

    void ChangeExitColor() {
        exit.GetComponent<MeshRenderer>().material = mat;
    }

    void DestroyBarriers() {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");

        foreach (GameObject barrier in barriers)
        {
            Destroy(barrier);
        }
    }

    void AddExit() {
        // add trigger to exit
        //trigger.enabled = true;
    }

    // https://forum.unity.com/threads/how-to-access-what-gameobject-is-inside-an-xr-socket-interactor.894883/

    bool CheckCubes() {
        // the receptacles only accept a specific cube, so if we know they're all filled, then unlock the door
        GameObject[] receptacles = GameObject.FindGameObjectsWithTag("Receptacle");
        bool filled = true;
        Debug.Log("numb recep: " + receptacles.Length);
        foreach (GameObject receptacle in receptacles)
        {
            XRSocketInteractor socket = receptacle.GetComponent<XRSocketInteractor>();

            IXRSelectInteractable objName = socket.GetOldestInteractableSelected();
            //GameObject obj = objName.transform;
            if (objName == null) {
                filled = false;
                break;
            }
        }
        return filled;
    }
}
