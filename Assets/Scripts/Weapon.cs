using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private Collider collider;
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
        collider.enabled = true;
    }

    public void StopDamage() {
        collider.enabled = false;
    }
}
