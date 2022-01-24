using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthReadoutDemo : MonoBehaviour
{
    private TMP_Text display;
    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        display.text = "HEALTH: 100";
    }
}
