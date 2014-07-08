using UnityEngine;
using System.Collections;

public static class PatrolLocations {

    private static Vector3[][] locations = new Vector3[][] {
            new Vector3[] { new Vector3(15, 1, -15), new Vector3(15, 1, 15), new Vector3(-15, 1, 15), new Vector3(-15, 1, -15)},
            new Vector3[] { new Vector3(-45, 0, 35), new Vector3(-45, 0, -5), new Vector3(-5, 0, 25), new Vector3(45, 0, 35), new Vector3(15, 0, -5), new Vector3(45, 0, -45)},
            new Vector3[] { new Vector3(-65, 0, -15), new Vector3(65, 0, -15), new Vector3(-65, 0, 15), new Vector3(65, 0, 15), new Vector3(15, 0, 65), new Vector3(15, 0, -65), new Vector3(-15, 0, 65), new Vector3(-15, 0, -65)},
            new Vector3[] { new Vector3(-55, 0, 55), new Vector3(-55, 0, -55), new Vector3(55, 0, 55), new Vector3(55, 0, -55), new Vector3(35, 0, -35), new Vector3(35, 0, 35), new Vector3(-35, 0, -35), new Vector3(-35, 0, 35)},
            new Vector3[] { new Vector3(-65, 0, 35), new Vector3(65, 0, 45), new Vector3(65, 0, 15), new Vector3(45, 0, -55), new Vector3(-5, 0, -5), new Vector3(-65, 0, 5), new Vector3(-55, 0, -65)},
            new Vector3[] { new Vector3(-15, 0, 65), new Vector3(-65, 0, 35), new Vector3(5, 0, 15), new Vector3(-35, 0, -5), new Vector3(-5, 0, -55), new Vector3(45, 0, -45), new Vector3(65, 0, 35)}
        };

    //assumes levels are in order; level 1/level index 1  = array index 0. If this becomes no longer the case, need to map levels to index here in this class
    public static Vector3[] GetLevelLocations(int i) {
        return locations[i-1];
    }
}
