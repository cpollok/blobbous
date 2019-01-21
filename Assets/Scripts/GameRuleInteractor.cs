using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRulesNotFoundException : Exception {
    public GameRulesNotFoundException(string gameRulesName) : base("Couldn't find GameObject containing game rules of type: " + gameRulesName) { }
}

public class GameRuleInteractor<T> : MonoBehaviour where T : GameRules {
    protected T gameRules;

	protected virtual void Awake() {
        gameRules = GameObject.FindObjectOfType<T>();

        if (!gameRules) {
            throw new GameRulesNotFoundException(typeof(T).Name);
        }
    }
}
