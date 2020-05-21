using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementBehaviour {
    Ignore,
    Chase,
    Distance
}

public class BlobBehaviour : GameRuleInteractor<GameRules> {

    [SerializeField] private EnemyInfo info;
    [SerializeField] protected Animator animator;
    [SerializeField] private GameObject gasPrefab;

    [SerializeField] private MovementBehaviour movementBehaviour = MovementBehaviour.Ignore;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected float turnSpeed = 1;
    [SerializeField] private float facingAngle = 15f;
    [SerializeField] private Color gasColor;

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

    public void GetHit() {
        Die();
    }

    public void GetHitByGas(GasType type) {
        switch (type) {
            case GasType.Harmless:
                break;
            case GasType.Acid:
                Die();
                break;
            case GasType.Glue:
                break;
            case GasType.Ink:
                break;
            default:
                break;
        }
    }

    private void Die() {
        gameRules.AwardPoints(info.PointValue);
        GameObject gasCloud = Instantiate(gasPrefab, this.transform.position, this.transform.rotation);
        gasCloud.GetComponent<GasCloud>().SetColor(gasColor);
        Destroy(gameObject);
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

    protected virtual void OnCollisionEnter(Collision collision) {
        gameRules.HandleCollision(this.gameObject, collision.collider);
    }
}
