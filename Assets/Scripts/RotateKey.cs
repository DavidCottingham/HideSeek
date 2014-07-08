using UnityEngine;
using System.Collections;

public class RotateKey : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, Space.World);
	}
}
