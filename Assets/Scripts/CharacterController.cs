using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : GameRuleInteractor<GameRules> {

    [SerializeField] private Animator animator;
    [SerializeField] private Transform mountingPoints;

    [SerializeField] private GameObject weapon;
    private Weapon weaponController;

    [SerializeField] private float staggerTime = 1.5f;
    private float staggerStart = -1;
    private bool stagger = false;

    private bool weaponSwung = false;
    private float lastTimeSwung = -1;

    private bool dead = false;

    void Start () {
        gameRules.SetPlayerCharacter(this.gameObject);
        weaponController = weapon.GetComponent<Weapon>();
	}
	
	void Update () {
        CheckStagger();
        CheckSwung();
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

    public void Move(float x, float z) {
        if (stagger || weaponSwung || dead) {
            return;
        }
        Turn(x, z);
        // Move towards goal position.
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(x, 0, z), Time.deltaTime);
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
        Debug.Log("Mounting!");
        EnemyInfo enemyInfo = mountee.GetComponent<EnemyInfo>();
        if (enemyInfo) {
            Debug.Log("Mounting enemy!");
            switch (enemyInfo.Type) {
                case EnemyType.Green:
                    break;
                case EnemyType.Blue:
                    Debug.Log("Mounting blue blob!");
                    Transform biterMount = mountingPoints.Find("Biter");
                    mountee.transform.parent = biterMount;
                    break;
                default:
                    break;
            }
        }
    }

    public void Die() {
        dead = true;
        animator.SetTrigger("BiterHit");
        LeaveWeapons();
    }

    private void LeaveWeapons() {
        Transform weaponsTF = mountingPoints.Find("Weapons");
        Weapon[] weapons = weaponsTF.GetComponentsInChildren<Weapon>();
        foreach (Weapon w in weapons) {
            //w.transform.parent = null;
            Destroy(w.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!stagger) {
            animator.SetFloat("BumpSpeed", 1f / staggerTime);
            animator.SetTrigger("Bump");
            stagger = true;
        }
    }
}
