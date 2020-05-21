using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private Collider bladeCollider;
    [SerializeField] private AudioSource swingSound;
    public AnimationClip swingAnimation;

    private bool damage = false;

	void Start () {
        animator = GetComponent<Animator>();
        GetExhaustLength();
	}
	
	void Update () {
		
	}

    public float GetExhaustLength() {
        return swingAnimation.length;
    }

    public void Swing() {
        animator.SetTrigger("Swing");
        bladeCollider.enabled = true;
        swingSound.PlayDelayed(0.05f);
    }

    public void StopDamage() {
        bladeCollider.enabled = false;
    }
}
