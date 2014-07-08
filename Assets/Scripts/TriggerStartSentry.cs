using UnityEngine;
using System.Collections;

public class TriggerStartSentry : MonoBehaviour {

    public delegate void StartSentryDelegate();
    public static event StartSentryDelegate StartSentry;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (StartSentry != null) {
                StartSentry();
            }
            Destroy(gameObject);
        }
    }
}
