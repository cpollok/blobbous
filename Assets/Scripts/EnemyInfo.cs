using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType {
    Green,
    Blue
}

public class EnemyInfo : CharacterInfo {
    [SerializeField] private EnemyType type;
    [SerializeField] private int pointValue;
    public int PointValue { get { return pointValue; } }

    public EnemyType Type {
        get {
            return type;
        }
    }

    //public void GetHit() {
    //    Destroy(this.gameObject);
    //}
}
