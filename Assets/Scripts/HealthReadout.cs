using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthReadout : MonoBehaviour
{
    private PlayerHealth playerH;
    private TMP_Text display;
    // Start is called before the first frame update
    void Start()
    {
        playerH = FindObjectOfType<PlayerHealth>();
        display = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        display.text = "HEALTH: " + Mathf.Round(playerH.playerHealth);
    }
}
