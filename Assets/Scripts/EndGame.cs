using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

    void Start() {
        Invoke("GoToMenu", 3f);
    }

    void OnGUI() {
        GUI.Label(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 12, 140, 24), "Thanks for playing!");
    }

    void GoToMenu() {
        Application.LoadLevel(0);
    }
}
