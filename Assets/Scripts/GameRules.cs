using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRules : MonoBehaviour {

    [SerializeField] private Canvas ui;

    private GameObject playerCharacter;
    public GameObject PlayerCharacter { get { return playerCharacter;}}

    [SerializeField]
    private GameObject[] blobPrefabs;
    public GameObject[] BlobPrefabs { get { return blobPrefabs;}}

    [SerializeField]
    private float[] blobSpawnRates;
    public float[] BlobSpawnRates { get { return blobSpawnRates; } }

    public void PlayerDead() {
        ui.enabled = true;
    }

    public void SetPlayerCharacter(GameObject character) {
        playerCharacter = character;
    }

    public void Reload() {
        SceneManager.LoadScene(0);
    }
}
