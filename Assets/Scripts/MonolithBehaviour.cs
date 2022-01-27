using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class MonolithBehaviour : MonoBehaviour
{
    // 'this' is the calling object duh!

    public float speed;
    public float maxDistFromPillar;
    public GameObject pillar;
    public float minAngle;
    public float maxAngle;

    private GameManager manager;
    private LineRenderer lineRenderer;
    private int segments = 100;
    private Renderer chaserRenderer;
    private InputDevice leftHand;
    private InputDevice rightHand;
    private AudioSource movementAudio;

    private InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice;
    private InputDeviceCharacteristics rightHandCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        movementAudio = GetComponent<AudioSource>();
        movementAudio.loop = true;
        movementAudio.Stop();
        GameObject myLine = new GameObject();
        myLine.transform.position = new Vector3(0,0,0);
        lineRenderer = myLine.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // "Particles/Alpha Blended Premultiply"
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = segments;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color (1.0f, 0.0f, 1.0f, 1f), 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        chaserRenderer = GetComponent<Renderer>();

        /*Vector3 v = new Vector3(1,10,1);
        Debug.Log("ang(should be 45): " + GetAngleFromVector(v));
        v = new Vector3(-1,15,0);
        Debug.Log("ang (should be 270): " + GetAngleFromVector(v));*/
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.gameEnded) {
            this.enabled = false;
        }

        DrawSineWave(pillar.transform.position, transform.position, 0.8f, 0.5f, 3.0f, 100);

        bool seen = false;
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
        float x = v.z;
        if (x >= 1.0f) {
            seen = true;
        } else {
            VRPortalRenderer r = FindObjectOfType<VRPortalRenderer>();
            if (r != null) {
                seen = r.IsPosRendered(transform.position);
            }
            
        }

        //if (chaserRenderer.isVisible) {
        bool moved = false;
        if (seen) {
            movementAudio.loop = false;
            movementAudio.Stop();
        } else {
            // Move our position a step closer to the target if within range of pillar (and also if target is within range of the pillar)
            if (Vector3.Distance(this.transform.position, pillar.transform.position) < maxDistFromPillar) {
                float step =  speed * Time.deltaTime; // calculate distance to move

                // if player within range it chases them, if not it retreats to the pillar
                Vector3 targetPos;

                if (Vector3.Distance(pillar.transform.position, Camera.main.transform.position) < maxDistFromPillar) {
                    // chase player
                    targetPos = Vector3.MoveTowards(this.transform.position, Camera.main.transform.position, step);
                    moved = true;
                } else {
                    // retreat
                    if (Vector3.Distance(this.transform.position, pillar.transform.position) > 1) {
                        targetPos = Vector3.MoveTowards(this.transform.position, pillar.transform.position, step);
                        moved = true;
                    } else {
                        targetPos = this.transform.position;
                    }
                }

                // this could make the chaser go out of range, so only move it if it remains within max dist after moving
                
                if (Vector3.Distance(targetPos, pillar.transform.position) < maxDistFromPillar
                    //&& GetAngleFromVector(targetPos - pillar.transform.position) >= minAngle
                    //&& GetAngleFromVector(targetPos - pillar.transform.position) <= maxAngle
                ) {
                    transform.position = targetPos;
                    if (!movementAudio.isPlaying && moved) {
                        movementAudio.volume = 1.2f;
                        movementAudio.loop = true;
                        movementAudio.Play();
                    }
                    
                }
                
            }

            // rotate to face player
            Vector3 targetDir = Camera.main.transform.position - transform.position;
            // get the cross product, then normalise.  This returns between -1 and 1.  Then multiply by 180 to get val between -180 and 180
            float fAngle = Vector3.Cross(targetDir.normalized,transform.forward.normalized).y;
            fAngle *= 180.0f;
            transform.Rotate(0, fAngle, 0, Space.World);
        }

        float distFromPlayer = Vector3.Distance(this.transform.position, Camera.main.transform.position);

        if (distFromPlayer < 3.0f) {
            float norm = 3.0f - distFromPlayer;
            float amplitude = norm * 0.33f;

            var leftControllers = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(leftHandCharacteristics, leftControllers);
            if (leftControllers.Count > 0)
            {
                leftHand = leftControllers[0];
                leftHand.SendHapticImpulse(0, amplitude, 0.5f);
            }
        
            var rightControllers = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(rightHandCharacteristics, rightControllers);
            if (rightControllers.Count > 0)
            {
                rightHand = rightControllers[0];
                rightHand.SendHapticImpulse(0, amplitude, 0.5f);
            }
        }
    }

    // calculates positions wrt sine wave for segments of line renderer

    void DrawSineWave(Vector3 startPoint, Vector3 endPoint, float amplitude, float frequency, float movementSpeed, int points) {
        float x = startPoint.x;
        float y;
        float z = startPoint.z;
        float Tau = 2 * Mathf.PI;
        float dist = Vector3.Distance(startPoint, endPoint);
        Vector3 temp = startPoint;

        for (int i = 0; i < points; i++) {
            temp = Vector3.MoveTowards(temp, endPoint, (dist / (float)points));
            x = temp.x;
            z = temp.z;
            
            //y = amplitude * Mathf.Sin((Tau *  frequency * i * (dist / (float)points)) + (Time.timeSinceLevelLoad * movementSpeed)); // constant
            y = amplitude * Mathf.Sin( (Tau *  (6 * (frequency / dist)) * (i * (dist / (float)points))) + (Time.timeSinceLevelLoad * movementSpeed) ); // wavelength increases with distance
            y += temp.y;
            lineRenderer.SetPosition(i, new Vector3(x, y, z));
        }
    }

    // convert vector to angle (remember use x & z, not x & y) !!
    float GetAngleFromVector(Vector3 vect) {
        float angle = 0.0f;
        float x = vect.x;
        float z = vect.z;
        angle = Mathf.Atan2(x, z) * Mathf.Rad2Deg;
        if (angle < 0) {
            angle += 360;
        }
        return angle;
    }
}
