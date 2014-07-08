using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    void Start() {
        Screen.lockCursor = false;
    }

    void OnGUI() {
        GUI.Label(new Rect(Screen.width / 2 - 23, Screen.height / 2 - 200, 46, 24), "Escape");

        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));
        if (GUI.Button(new Rect(0, 0, 200, 40), "Start Game")) {
            Application.LoadLevel(1);
        }
        if (GUI.Button(new Rect(0, 42, 200, 40), "Quit")) {
            Application.Quit();
        }
        GUI.Label(new Rect(20, 90, 200, 20), "Made by David Cottingham");
        GUI.EndGroup();

        int guiWidth = 460;
        GUI.BeginGroup(new Rect(Screen.width / 2 - guiWidth / 2, Screen.height / 2 + 50, guiWidth, 800));

        GUI.Label(new Rect(0, 0, guiWidth, 20), "Sounds: ");
        GUI.Label(new Rect(0, 15, guiWidth, 20), "boink 0019 by davepape");
        GUI.Label(new Rect(20, 30, guiWidth, 20), "http://www.freesound.org/people/davepape/sounds/9942/");

        GUI.Label(new Rect(0, 60, guiWidth, 20), "buttonchime02up by JustinBW");
        GUI.Label(new Rect(20, 75, guiWidth, 20), "http://www.freesound.org/people/JustinBW/sounds/80921/");

        GUI.Label(new Rect(0, 105, guiWidth, 20), "R2 talk by mik300z");
        GUI.Label(new Rect(20, 120, guiWidth, 20), "http://www.freesound.org/people/mik300z/sounds/103525/");

        GUI.Label(new Rect(0, 150, guiWidth, 20), "Music:");
        GUI.Label(new Rect(0, 165, guiWidth, 20), "Basement Floor by Kevin MacLeod");
        GUI.Label(new Rect(20, 180, guiWidth, 20), "http://incompetech.com/music/royalty-free/index.html?isrc=USUAN1100538");

        GUI.EndGroup();
    }
}
