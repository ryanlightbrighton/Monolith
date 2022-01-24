using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameEnded = false;
    public float restartDelay;

    private GameObject gameOverUI;
    private GameObject winUI;
    private GameObject player;
    private GameObject spawnPos;

    void Start() {
        gameOverUI = GameObject.FindWithTag("UI Game Over");
        gameOverUI.SetActive(false);
        winUI = GameObject.FindWithTag("UI Winner");
        if (winUI != null) {
            winUI.SetActive(false);
        }
        player = GameObject.FindWithTag("Player");
        spawnPos = GameObject.FindWithTag("Respawn");
        player.transform.position = spawnPos.transform.position;
    } 

    // Start is called before the first frame update
    public void EndGame() {
        if (! gameEnded) {
            gameEnded = true;
            Debug.Log("GAME OVER");
            gameOverUI.SetActive(true);
            Invoke(nameof(Restart), restartDelay);
        }
    }

    void Restart() {
        gameOverUI.SetActive(false);
        SceneManager.LoadScene("Intro");
    }
}
