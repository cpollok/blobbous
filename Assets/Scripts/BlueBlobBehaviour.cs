using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBlobBehaviour : BlobBehaviour {

    private enum State {
        Default,
        PounceReady,
        Pouncing,
        Eating
    }
    
    [SerializeField] private float pounceRange = 20f;
    [SerializeField] private float pounceRangeMult = 0.75f;

    [SerializeField] private State state = State.Default;

    private Vector3 pounceStartPos;

    protected override void Update() {
        SetSpeed();
        base.Update();

        switch (state) {
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
        switch (state) {
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
        if (DistanceToTarget() < pounceRange) {
            return true;
        }
        return false;
    }

    private void StartPounce() {
        state = State.PounceReady;
        pounceStartPos = transform.position;
        animator.SetTrigger("PounceReady");
    }

    private void Pounce() {
        state = State.Pouncing;
        animator.SetTrigger("PounceJump");
    }

    private void EndPounce() {
        state = State.Default;
        animator.SetTrigger("PounceEnd");
    }
    protected override void OnCollisionEnter(Collision collision) {
        switch (state) {
            case State.PounceReady:
                break;
            case State.Pouncing:
                CharacterInfo info = collision.gameObject.GetComponent<CharacterInfo>();
                if (!info) {
                    Debug.Log("Collided with something unusual...");
                    break;
                }
                if (info.Faction == Faction.Player) {
                    GetComponent<Collider>().enabled = false;
                    state = State.Eating;
                    CharacterController character = collision.gameObject.GetComponent<CharacterController>();
                    character.Mount(this.gameObject);
                    character.Die();
                    this.transform.localPosition = Vector3.zero;
                    this.TurnInstant(collision.transform.position - transform.position);
                    animator.SetTrigger("PounceHit");
                }
                else {
                    // Slow down?
                    // Handle pouncing collision with special blobs.
                }
                break;
            case State.Eating:
                break;
            default:
                base.OnCollisionEnter(collision);
                break;
        }
    }
}
