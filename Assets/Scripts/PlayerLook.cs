using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour {

    private float lookSensitivty = 4.1f;
    private float xRotation;
    public float yRotation;
    private float currXRotation;
    private float currYRotation;
    private float xRotVelocity = 1f;
    private float yRotVelocity = 1f;
    private float lookSmoothDamp = 0.05f;

	// Use this for initialization
	void Start () {
        //start camera rotations not at 0, but player GO rotation
        xRotation = transform.parent.eulerAngles.x;
        currXRotation = xRotation;
        yRotation = transform.parent.eulerAngles.y;
        currYRotation = yRotation;
    }
	
	// Update is called once per frame
	void Update () {
        xRotation -= Input.GetAxis("Mouse Y") * lookSensitivty;
        yRotation += Input.GetAxis("Mouse X") * lookSensitivty;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        currXRotation = Mathf.SmoothDamp(currXRotation, xRotation, ref xRotVelocity, lookSmoothDamp);
        currYRotation = Mathf.SmoothDamp(currYRotation, yRotation, ref yRotVelocity, lookSmoothDamp);

        //difference in these 2 lines?
        transform.rotation = Quaternion.Euler(currXRotation, currYRotation, 0);
        //transform.eulerAngles = new Vector3(xRotation, yRotation, 0);
	}
}
