using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSwitch : MonoBehaviour {

    private void OnBecameInvisible() {
        Destroy(this.transform.parent.gameObject);
    }
}
