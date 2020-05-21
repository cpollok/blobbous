using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRules : MonoBehaviour {

    [SerializeField] private GameObject ui;
    [SerializeField] private CollisionRules collisionRules;

    private Text pointsValue;
    private Text comboValueText;
    private Text timeText;
    private int currentPoints;
    private bool pointsLocked = false;
    private bool paused = false;
    public bool Paused { get { return paused; } }

    private float roundStartTime;
    public float RoundStartTime { get { return roundStartTime; } }

    private List<int> combo;
    private int ComboValue {
        get {
            int value = 0;
            foreach (int p in combo) {
                value += p;
            }
            return value * combo.Count;
        }
    }

    [SerializeField] private float comboTimeMax = 1.0f;
    private float comboTimeStart = -1;

    private GameObject playerCharacter;
    public GameObject PlayerCharacter { get { return playerCharacter;}}

    [SerializeField]
    private GameObject[] blobPrefabs;
    public GameObject[] BlobPrefabs { get { return blobPrefabs;}}

    [SerializeField]
    private float[] blobSpawnRates;
    public float[] BlobSpawnRates { get { return blobSpawnRates; } }

    protected void Start() {
        pointsValue = ui.transform.Find("PointsValue").GetComponent<Text>();
        comboValueText = ui.transform.Find("ComboValue").GetComponent<Text>();
        timeText = ui.transform.Find("Time").GetComponent<Text>();
        timeText.text = "0.00";

        roundStartTime = Time.time;
        currentPoints = 0;
        combo = new List<int>();
        pointsLocked = false;
        paused = false;
    }

    private void Awake() {
        Resume();
    }

    protected void Update() {
        if (Input.GetButtonDown("Pause")) {
            GameObject gameOver = ui.transform.Find("GameOverScreen").gameObject;
            if (!gameOver.activeSelf) {
                if (paused) {
                    Resume();
                }
                else {
                    Pause();
                }
            }
        }
        if (!pointsLocked) {
            timeText.text = (Time.time - roundStartTime).ToString("F2");
        }
        CheckCombo();
    }

    public void PlayerDead() {
        pointsLocked = true;
        GameObject gameOver = ui.transform.Find("GameOverScreen").gameObject;
        gameOver.SetActive(true);
    }

    public void SetPlayerCharacter(GameObject character) {
        playerCharacter = character;
    }

    public void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause() {
        paused = true;
        pointsLocked = true;
        Time.timeScale = 0;
        GameObject pause = ui.transform.Find("PauseScreen").gameObject;
        pause.SetActive(true);
    }

    public void Resume() {
        paused = false;
        GameObject pause = ui.transform.Find("PauseScreen").gameObject;
        pause.SetActive(false);
        Time.timeScale = 1;
        pointsLocked = false;
    }

    public void HandleCollision(GameObject o, Collider other) {
        collisionRules.HandleCollision(o, other);
    }

    private void CheckCombo() {
        if (comboTimeStart + comboTimeMax < Time.time) {
            currentPoints += ComboValue;
            combo.Clear();
            comboValueText.text = "";
            pointsValue.text = currentPoints.ToString();
        }
    }

    public void AwardPoints(int amount) {
        if (pointsLocked) {
            return;
        }

        combo.Add(amount);
        comboValueText.text = "+ " + ComboValue.ToString();

        if (comboTimeStart + comboTimeMax < Time.time) {
            comboTimeStart = Time.time;
        }
    }
}
