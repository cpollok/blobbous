using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction {
    Player,
    Enemies
}

public class CharacterInfo : MonoBehaviour {

    [SerializeField]
    private Faction faction;

    private Faction Faction {
        get {
            return faction;
        }
    }
}
