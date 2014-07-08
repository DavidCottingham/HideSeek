using UnityEngine;
using System.Collections;

public class DoorActivate : MonoBehaviour {

    public delegate void ExitLevelDelegate();
    public static ExitLevelDelegate ExitLevel;

    public Material glowMaterial; //set in inspector

    private bool keyPickedUp = false;

	// Use this for initialization
	void Start () {

	}

    void OnEnable() {
        PickupKey.KeyPickedUp += MakeActive;
    }

    void OnDisable() {
        PickupKey.KeyPickedUp -= MakeActive;
    }

    void MakeActive() {
        renderer.material = glowMaterial;
        collider.enabled = true;
        keyPickedUp = true;
    }

    void OnTriggerEnter(Collider other) {
        if (keyPickedUp) {
            if (other.tag == "Player") {
                if (ExitLevel != null) {
                    ExitLevel();
                }
            }
        }
    }
}
