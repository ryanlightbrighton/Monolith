using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class MonolithDemo : MonoBehaviour
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
    }

    // Update is called once per frame
    void Update()
    {
        DrawSineWave(pillar.transform.position, transform.position, 0.8f, 1.0f);

        // rotate to face player
        Vector3 targetDir = Camera.main.transform.position - transform.position;
        // get the cross product, then normalise.  This returns between -1 and 1.  Then multiply by 180 to get val between -180 and 180
        float fAngle = Vector3.Cross(targetDir.normalized,transform.forward.normalized).y;
        fAngle *= 180.0f;
        transform.Rotate(0, fAngle, 0, Space.World);

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

    void DrawSineWave(Vector3 startPoint, Vector3 endPoint, float amplitude, float waveSpeed) {
        float x = startPoint.x;
        float y;
        float z = startPoint.z;
        
        float dist = Vector3.Distance(startPoint, endPoint);
        float wavelength = dist / 5.0f;  // 5 wavecrests (20 segments each)
        float waveNumber = (2 * Mathf.PI) / wavelength;
        float angularFreq = waveNumber * waveSpeed;
        Vector3 temp = startPoint;

        for (int i = 0; i < segments; i++)
        {
            temp = Vector3.MoveTowards(temp, endPoint, dist / segments); // segments was 100.0f
            x = temp.x;
            z = temp.z;
            y = amplitude * Mathf.Sin((waveNumber * x) + (angularFreq * Time.time));
            lineRenderer.SetPosition(i, new Vector3(x, y + 1.8f, z));
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

