using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementBehaviour {
    Ignore,
    Chase,
    Distance
}

public class BlobBehaviour : GameRuleInteractor<GameRules> {

    [SerializeField] protected Animator animator;

    [SerializeField] private MovementBehaviour movementBehaviour = MovementBehaviour.Ignore;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float turnSpeed = 1;
    [SerializeField] private float facingAngle = 15f;

    [SerializeField] protected Transform target;

    private float initialSpeed;
    protected float InitialSpeed { get { return initialSpeed; } }

    private float initialTurnSpeed;
    protected float InitialTurnSpeed { get { return initialTurnSpeed; } }

    private void Start() {
        if (!target) {
            target = gameRules.PlayerCharacter.transform;
        }
        initialSpeed = speed;
        initialTurnSpeed = turnSpeed;
    }

    protected virtual void Update () {
        Move();
	}

    private void Move() {
        switch (movementBehaviour) {
            case MovementBehaviour.Ignore:
                MoveIgnore();
                break;
            case MovementBehaviour.Chase:
                MoveChase();
                break;
            case MovementBehaviour.Distance:
                MoveDistance();
                break;
            default:
                MoveIgnore();
                break;
        }
    }

    private void MoveForward () {
        this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * speed;
    }

    private void MoveIgnore() {
        MoveForward();
    }

    private void MoveChase() {
        TurnTowardsTarget();
        MoveForward();
    }

    private void MoveDistance() {

    }

    private void TurnTowardsTarget() {
        Vector3 targetV = target.position - transform.position;
        float yAngle = Vector3.SignedAngle(transform.forward, targetV.normalized, Vector3.up) * Time.deltaTime * turnSpeed;
        this.transform.Rotate(0f, yAngle, 0f);
    }

    public void TurnInstant(Vector3 direction) {
        this.transform.LookAt(this.transform.position + direction);
    }
    
    protected float DistanceToTarget() {
        return (target.position - transform.position).magnitude;
    }

    protected bool FacingTarget() {
        Vector3 targetV = target.transform.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, targetV);
        if (angleToTarget < facingAngle) {
            return true;
        }
        return false;
    }

    //protected virtual void OnCollisionEnter(Collision collision) {
    //    Vector3 otherPos = collision.collider.transform.position;
    //    Vector3 direction = this.transform.position - otherPos;
    //    TurnInstant(direction);
    //}

    protected virtual void OnTriggerEnter(Collider other) {
        gameRules.HandleCollision(this.gameObject, other);
        //Vector3 otherPos = other.transform.position;
        //Vector3 direction = this.transform.position - otherPos;
        //TurnInstant(direction);
    }
}
