﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] private Animator animator;
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
        damage = true;
    }

    public void StopDamage() {
        damage = false;
    }

    private void OnTriggerEnter(Collider other) {
        EnemyInfo enemy = other.GetComponent<EnemyInfo>();
        if (enemy && damage) {
            enemy.GetHit();
        }
    }
}
