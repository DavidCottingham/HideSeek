using UnityEngine;
using System.Collections;

public class PlayerPositionManager : MonoBehaviour {

    public delegate void ReportDelegate(Vector3 position);
    public static event ReportDelegate PositionChanged;

    private Vector3 previousPosition;

    void Start() {
        previousPosition = transform.position;
        InvokeRepeating("MovedPosition", 0.1f, 0.1f);
    }

    void MovedPosition() {
        if (transform.position != previousPosition) {
            if (PositionChanged != null) {
                PositionChanged(transform.position);
                previousPosition = transform.position;
            }
        }
    }
}
