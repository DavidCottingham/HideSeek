using UnityEngine;
using System.Collections;

public class PickupKey : MonoBehaviour {

    public delegate void KeyDelegate();
    public static event KeyDelegate KeyPickedUp;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (KeyPickedUp != null) {
                KeyPickedUp();
            }
            Destroy(gameObject);
        }
    }
}
