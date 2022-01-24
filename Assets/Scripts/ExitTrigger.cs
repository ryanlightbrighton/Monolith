using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public string levelName;
    public float restartDelay;

    private GameObject winUI;

    void Start() {
        winUI = GameObject.FindWithTag("UI Winner");
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Scene currScene = SceneManager.GetActiveScene();
            string sceneName = currScene.name;
            if (sceneName == "Level_2") {
                winUI.SetActive(true);
                Invoke(nameof(Restart), restartDelay);
            } else {
                SceneManager.LoadScene(levelName);
            }  
        }
    }

    void Restart() {
        winUI.SetActive(false);
        SceneManager.LoadScene("Intro");
    }
}
