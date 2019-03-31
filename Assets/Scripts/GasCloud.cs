using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GasType {
    Acid,
    Glue,
    Ink
}

public class GasCloud : GameRuleInteractor<GameRules> {

    private GasType gasType = GasType.Acid;

    public GasType GasType { get {return gasType; } }

    private void OnParticleCollision(GameObject other) {
        gameRules.HandleCollision(this.gameObject, other.GetComponent<Collider>());
    }
}
