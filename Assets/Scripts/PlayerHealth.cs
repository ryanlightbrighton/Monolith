using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public float playerHealth;
    public float damageMultiplier;
    private float ticker;
    public AudioSource damageAudio;
    public AudioSource deathAudio;

    private GameManager manager;

    // Start is called before the first frame update
    void Start() {
        ticker = Time.time;

        manager = FindObjectOfType<GameManager>();

        GetComponents<AudioSource>();
        damageAudio.loop = false;
        damageAudio.Stop();

        // set player pos

        this.transform.position = new Vector3(0, 0, -50);
    }

    // Update is called once per frame (ticker will apply damage once a second)
    void Update() {
        if (Time.time >= ticker + 1 && !manager.gameEnded) {
            ticker = Time.time;

            // damage checks here

            List<GameObject> enemies = GetEnemies();

            float damage = CalcDamage(enemies);

            if (damage > 0.0f) {
                damageAudio.Play();
            }

            playerHealth -= damage;

            playerHealth = Mathf.Max(playerHealth, 0.0f);
            //Debug.Log("playerHealth: " + playerHealth);
            if (IsPlayerDead(playerHealth)) {
                deathAudio.Play();
                this.enabled = false;
                manager.EndGame();
            }
        }
    }

    // get enemies in range (let's say 3m)
    List<GameObject> GetEnemies() {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> inRange = new List<GameObject>();

        for (int i = 0; i < enemies.Length; i++) {
            if (Vector3.Distance(this.transform.position, enemies[i].transform.position) < 3) {
                inRange.Add(enemies[i]);
            }
        }

        return inRange;
    }

    float CalcDamage(List<GameObject> enemies) {

        float dam = 0.0f;
        foreach (GameObject enemy in enemies) {
            // check dist and add damage based on dist

            float distFromPlayer = Vector3.Distance(this.transform.position, enemy.transform.position);

            if (distFromPlayer < 3.0f) {
                float norm = 3.0f - distFromPlayer;
                dam += norm * 0.33f * damageMultiplier;
            }
        }
        return dam;
    }

    bool IsPlayerDead(float health) {
        if (health <= 0.0f) {
            return true;
        }
        return false;
    }
}
