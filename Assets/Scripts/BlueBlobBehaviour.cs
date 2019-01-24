using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBlobBehaviour : BlobBehaviour {

    public enum State {
        Default,
        PounceReady,
        Pouncing,
        Eating
    }
    
    [SerializeField] private float pounceRange = 20f;
    [SerializeField] private float pounceRangeMult = 0.75f;

    [SerializeField] private State currentState = State.Default;
    public State CurrentState { get { return currentState; } }

    private Vector3 pounceStartPos;

    protected override void Update() {
        SetSpeed();
        base.Update();

        switch (currentState) {
            case State.Default:
                if (InPounceRange() && FacingTarget()) {
                    StartPounce();
                }
                break;
            case State.Pouncing:
                if ((transform.position - pounceStartPos).magnitude > pounceRange) {
                    EndPounce();
                }
                break;
            default:
                break;
        }
    }

    private void SetSpeed() {
        switch (currentState) {
            case State.Default:
                speed = InitialSpeed;
                turnSpeed = InitialTurnSpeed;
                break;
            case State.PounceReady:
                speed = 0;
                turnSpeed = InitialTurnSpeed * 2;
                break;
            case State.Pouncing:
                speed = InitialSpeed * 3;
                turnSpeed = 0;
                break;
            case State.Eating:
                speed = 0;
                turnSpeed = 0;
                break;
            default:
                speed = InitialSpeed;
                turnSpeed = InitialTurnSpeed;
                break;
        }
    }

    private bool InPounceRange() {
        if (DistanceToTarget() < pounceRange * pounceRangeMult) {
            return true;
        }
        return false;
    }

    private void StartPounce() {
        currentState = State.PounceReady;
        pounceStartPos = transform.position;
        animator.SetTrigger("PounceReady");
    }

    private void Pounce() {
        currentState = State.Pouncing;
        animator.SetTrigger("PounceJump");
    }

    private void EndPounce() {
        currentState = State.Default;
        animator.SetTrigger("PounceEnd");
    }

    public void StartEating() {
        GetComponent<Collider>().enabled = false;
        currentState = State.Eating;
        animator.SetTrigger("PounceHit");
    }
}
