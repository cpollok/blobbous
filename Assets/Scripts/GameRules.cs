using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour {

    private GameObject playerCharacter;
    public GameObject PlayerCharacter { get { return playerCharacter;}}

    private GameObject[] blobPrefabs;
    public GameObject[] BlobPrefabs { get { return blobPrefabs;}}

    public void SetPlayerCharacter(GameObject character) {
        playerCharacter = character;
    }
}
