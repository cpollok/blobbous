using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobBehaviour : MonoBehaviour {

    [SerializeField] private float speed = 5;
    
	void Update () {
        Move();
	}

    void Move() {
        // this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + this.transform.forward, Time.deltaTime * speed);
        this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime * speed;
    }

    private void Turn(Vector3 direction) {
        this.transform.LookAt(this.transform.position + direction);
    }

    private void OnCollisionEnter(Collision collision) {
        Vector3 otherPos = collision.collider.transform.position;
        Vector3 direction = this.transform.position - otherPos;
        Turn(direction);
    }
}
