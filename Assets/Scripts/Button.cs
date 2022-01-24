using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{

    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float deadZone = 0.025f;

    private bool isPressed;
    private Vector3 startPos;
    private ConfigurableJoint joint;

    public UnityEvent onPressed, onReleased;
    private AudioSource noise;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        joint = GetComponent<ConfigurableJoint>();
        noise = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (! isPressed && GetValue() + threshold >= 1) {
            Pressed();
        }
        if (isPressed && GetValue() - threshold <= 0) {
            Released();
        }
    }

    private float GetValue() {
        var val = Vector3.Distance(startPos, transform.localPosition) / joint.linearLimit.limit;

        if (Math.Abs(val) < deadZone) {
            val= 0;
        }
        return Mathf.Clamp(val, -1.0f, 1.0f);
    }

    private void Pressed() {
        noise.Play();
        isPressed = true;
        onPressed.Invoke();
        Debug.Log(this.transform.parent.gameObject.name + ": Pressed");
    }

    private void Released() {
        isPressed = false;
        onReleased.Invoke();
        Debug.Log(this.transform.parent.gameObject.name + ": Released");
    }
}
