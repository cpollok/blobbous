using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : GameRuleInteractor<GameRules> {

    [SerializeField] private Animator animator;
    [SerializeField] private Transform mountingPoints;

    [SerializeField] private GameObject weapon;
    private Weapon weaponController;

    [SerializeField] private float speed = 10;
    [SerializeField] private float currentSpeed;

    [SerializeField] private float staggerTime = 1.5f;
    private float staggerStart = -1;
    private bool stagger = false;

    private bool weaponSwung = false;
    private float lastTimeSwung = -1;

    private bool dead = false;
    public bool Dead { get { return dead; } }

    private bool sticky = false;
    private float stickyStart = -1;
    public float stickyFactor = 0.5f;
    public float stickyDuration = 3;
    public float stickyWearOffDuration = 2;

    [SerializeField]
    private FlashImage blindfold;
    private float blindDuration;
    private float blindStart = -1;
    private bool blinded = false;

    public bool NoControl { get { return gameRules.Paused; } }

    void Start () {
        gameRules.SetPlayerCharacter(this.gameObject);
        weaponController = weapon.GetComponent<Weapon>();
        currentSpeed = speed;
	}
	
	void Update () {
        CheckStagger();
        CheckSwung();
        CheckSticky();
    }

    void CheckStagger() {
        if (stagger) {
            if (staggerStart == -1) {
                staggerStart = Time.time;
            }
            if (Time.time - staggerStart > staggerTime) {
                stagger = false;
                staggerStart = -1;
            }
        }
    }

    void CheckSwung() {
        if (weaponSwung) {
            if (lastTimeSwung == -1) {
                lastTimeSwung = Time.time;
            }
            if (Time.time - lastTimeSwung > weaponController.GetExhaustLength()) {
                weaponSwung = false;
                lastTimeSwung = -1;
            }
        }
    }

    void CheckSticky() {
        if (sticky) {
            float stickyTime = Time.time - stickyStart;
            if (stickyTime < stickyDuration) {
                currentSpeed = speed * stickyFactor;
            }
            else {
                currentSpeed = Mathf.Lerp(stickyFactor * speed, speed, (stickyTime - stickyDuration) / stickyWearOffDuration);
                if (currentSpeed >= speed) {
                    currentSpeed = speed;
                    sticky = false;
                    stickyStart = -1;
                    GameObject glueEffect = transform.Find("GlueEffect").gameObject;
                    glueEffect.SetActive(false);
                }
            }
        }
    }

    void CheckBlind() {
        if (blinded) {
            if (Time.time >= blindStart + blindDuration) {
                blinded = false;
                blindfold.gameObject.SetActive(false);
            }
        }
    }

    public void Move(float x, float z) {
        if (stagger || weaponSwung || dead) {
            return;
        }
        Turn(x, z);
        // Move towards goal position.
        Vector3 direction = new Vector3(x, 0, z) - this.transform.position;
        if (direction.magnitude > 0.2) {
            this.transform.position = this.transform.position + direction.normalized * Time.deltaTime * currentSpeed;
        }
    }

    private void Turn(float x, float z) {
        // Turn towards goal position.
        this.transform.LookAt(new Vector3(x, 0, z));
    }

    public void SwingWeapon() {
        if (!(stagger || weaponSwung)) {
            weaponController.Swing();
            weaponSwung = true;
        }
    }

    public void Mount(GameObject mountee) {
        EnemyInfo enemyInfo = mountee.GetComponent<EnemyInfo>();
        if (enemyInfo) {
            switch (enemyInfo.Type) {
                case EnemyType.Green:
                    break;
                case EnemyType.Blue:
                    Transform biterMount = mountingPoints.Find("Biter");
                    mountee.transform.parent = biterMount;
                    mountee.transform.localPosition = Vector3.zero;
                    mountee.transform.localRotation = Quaternion.identity;
                    break;
                default:
                    break;
            }
        }
    }

    public void GetHitByGas(GasType type) {
        switch (type) {
            case GasType.Acid:
                Debug.Log("Damn Nazis!");
                Die();
                break;
            case GasType.Glue:
                GetSticky();
                break;
            case GasType.Ink:
                GetInked();
                break;
            default:
                break;
        }
    }

    public void Die() {
        GetComponent<Collider>().enabled = false;
        dead = true;
        animator.SetTrigger("BiterHit");
        LeaveWeapons();
        gameRules.PlayerDead();
    }

    public void GetSticky() {
        if (!sticky) {
            Debug.Log("Ewww! Sticky!");
            sticky = true;
            stickyStart = Time.time;
            GameObject glueEffect = transform.Find("GlueEffect").gameObject;
            glueEffect.SetActive(true);
        }
    }

    public void GetInked() {
        if (!blinded) {
            Debug.Log("Ahhhh! Who put the lights out?");
            blinded = true;
            blindStart = Time.time;
            blindfold.gameObject.SetActive(true);
            blindfold.BlinkBlack();
        }
    }

    private void LeaveWeapons() {
        Transform weaponsTF = mountingPoints.Find("Weapons");
        Weapon[] weapons = weaponsTF.GetComponentsInChildren<Weapon>();
        foreach (Weapon w in weapons) {
            //w.transform.parent = null;
            Destroy(w.gameObject);
        }
    }

    public void Stagger() {
        if (!stagger) {
            animator.SetFloat("BumpSpeed", 1f / staggerTime);
            animator.SetTrigger("Bump");
            stagger = true;
        }
    }
    protected virtual void OnCollisionEnter(Collision collision) {
        gameRules.HandleCollision(this.gameObject, collision.collider);
    }
}
