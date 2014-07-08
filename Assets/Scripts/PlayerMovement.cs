using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public Camera playerCam; //set in inspector

    private float speed = 10.0f;
    private Transform cameraTransform;
    private Vector3 moveDirection;
    private CharacterController controller;

	// Use this for initialization
	void Start () {
        cameraTransform = playerCam.transform;
        controller = GetComponent<CharacterController>();
	}

	void Update () {
        transform.eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y, 0);

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        controller.Move(moveDirection * Time.deltaTime);
	}
}
