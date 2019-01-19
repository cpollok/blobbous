using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public GameObject weapon;
    [SerializeField] private bool stagger = false;

    private float staggerTime = 1.5f;
    private float staggerStart = -1;

    private Weapon weaponController;
    private bool weaponSwung = false;
    private float lastTimeSwung = -1;
    
	void Start () {
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
        if (stagger || weaponSwung) {
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

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision Player!");
        // Bump
        stagger = true;
    }
}
