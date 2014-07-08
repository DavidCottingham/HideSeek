using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {

    public Transform sentry; //assigned in inspector
    public Texture sentryTutorialImage; //assigned in inspector
    public Texture escapeTutorialImage; //assigned in inspector
    private float sentryTutorialDisplayTime = 5.6f;
    private float escapeTutorialDisplayTime = 5.0f;

    void OnEnable() {
        TriggerStartSentry.StartSentry += SentryTutorialSetup;
        PickupKey.KeyPickedUp += EscapeTutorialSetup;
    }

    void OnDisable() {
        TriggerStartSentry.StartSentry -= SentryTutorialSetup;
        PickupKey.KeyPickedUp -= EscapeTutorialSetup;
    }

    void SentryTutorialSetup() {

        gameObject.GetComponent<SceneManager>().ArrestPlayerAndLookDisplayScreen(sentryTutorialDisplayTime, sentry.transform, sentryTutorialImage);
    }

    void EscapeTutorialSetup() {
        gameObject.GetComponent<SceneManager>().DisplayScreen(escapeTutorialImage, escapeTutorialDisplayTime);
    }
}
