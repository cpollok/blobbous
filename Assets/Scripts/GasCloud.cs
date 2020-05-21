using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GasType {
    Harmless,
    Acid,
    Glue,
    Ink
}

public class GasCloud : GameRuleInteractor<GameRules> {

    [SerializeField] private GasType gasType;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioSource deathSound;

    public GasType GasType { get {return gasType; } }

    private void Start() {
        Destroy(this.gameObject, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
    }

    private void OnParticleCollision(GameObject other) {
        gameRules.HandleCollision(this.gameObject, other.GetComponent<Collider>());
    }

    public void SetColor(Color color) {
        var psMain = particleSystem.main;
        psMain.startColor = color;
    }
}
