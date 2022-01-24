using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubes");

        foreach (GameObject cube in cubes) {
            BoxCollider c = cube.GetComponent<BoxCollider>();
            Physics.IgnoreCollision(c, controller);
        }

        GameObject[] shields = GameObject.FindGameObjectsWithTag("Shield");
        foreach (GameObject shield in shields) {
            BoxCollider s = shield.GetComponent<BoxCollider>();
            Physics.IgnoreCollision(s, controller);
        }
    }
}
