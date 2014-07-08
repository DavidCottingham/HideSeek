using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public GameObject player;
    public GameObject playerCamera;
    public Texture deathScreen;

    private float deathScreenShowTime = 4.0f;

    private bool lookAtSentry = false;
    private Transform lookAtTarget;
    private bool displayScreen = false;
    private Texture imageToDisplay;

    void Start() {
        Screen.lockCursor = true;
    }

    void OnEnable() {
        SentryAIMove.PlayerCaught += PlayerCaught;
        DoorActivate.ExitLevel += ExitLevel;
        PickupKey.KeyPickedUp += KeyGet;
    }

    void OnDisable() {
        SentryAIMove.PlayerCaught -= PlayerCaught;
        DoorActivate.ExitLevel -= ExitLevel;
        PickupKey.KeyPickedUp -= KeyGet;
    }

	void Update() {
        if (lookAtSentry) {
            playerCamera.transform.LookAt(lookAtTarget);
        }
        if (Input.GetKey("escape")) {
            Application.Quit();
        }
	}

    void OnGUI() {
        if (displayScreen) {
            Rect rect = new Rect(Screen.width / 2 - imageToDisplay.width / 2, Screen.height / 2 - imageToDisplay.height / 2, imageToDisplay.width, imageToDisplay.height);
            GUI.Label(rect, imageToDisplay);
        }
    }

    public void ArrestPlayerAndLook(float timeArrest, Transform lookTarget, float timeLook) {
        if (timeArrest > 0.0f) {
            //disable player look
            playerCamera.GetComponent<PlayerLook>().enabled = false;
            //disable player movement
            player.GetComponent<PlayerMovement>().enabled = false;
            Invoke("UnarrestPlayer", timeArrest);
        } 
        if (timeLook > 0.0f || lookTarget == null) {
            this.lookAtTarget = lookTarget;
            lookAtSentry = true;
            Invoke("PlayerStopLooking", timeLook);
        }
    }

    public void ArrestPlayerAndLook(float time, Transform lookTarget) {
        ArrestPlayerAndLook(time, lookTarget, time);
    }

    public void ArrestPlayer(float time) {
        ArrestPlayerAndLook(time, null, 0.0f);
    }

    public void PlayerLookAtTarget(Transform lookTarget, float time) {
        ArrestPlayerAndLook(0.0f, lookTarget, time);
    }

    private void UnarrestPlayer() {
        playerCamera.GetComponent<PlayerLook>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    private void PlayerStopLooking() {
        lookAtSentry = false;
    }

    public void DisplayScreen(Texture image, float time) {
        displayScreen = true;
        imageToDisplay = image;
        Invoke("StopDisplayScreen", time);
    }

    private void StopDisplayScreen() {
        displayScreen = false;
    }

    public void ArrestPlayerAndLookDisplayScreen(float timeArrest, Transform lookTarget, float timeLook, Texture image, float timeDisplay) {
        ArrestPlayerAndLook(timeArrest, lookTarget, timeLook);
        DisplayScreen(image, timeDisplay);
    }

    public void ArrestPlayerAndLookDisplayScreen(float time, Transform lookTarget, Texture image) {
        ArrestPlayerAndLookDisplayScreen(time, lookTarget, time, image, time);
    }

    private void PlayerCaught(Transform sentry) {
        ArrestPlayerAndLookDisplayScreen(deathScreenShowTime, sentry, deathScreen);
        Invoke("RestartScene", deathScreenShowTime);
    }

    private void RestartScene() {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void ExitLevel() {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    private void KeyGet() {
        audio.Play();
    }
}
