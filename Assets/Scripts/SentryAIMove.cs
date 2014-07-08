using UnityEngine;
using System.Collections;

public class SentryAIMove : MonoBehaviour {

    public Transform player; //assigned in inspector
    public delegate void CatchPlayerDelegate(Transform sentry);
    public static event CatchPlayerDelegate PlayerCaught;

    private NavMeshAgent agent;
    private bool allowPlayerFollow = true; //currently not used in context of anything
    //private bool startedPatrol = false; //currently not used
    private Vector3[] patrolLocations;
    private int prevPatrolLocation = -1;
    private Vector3 tutorialLocation = new Vector3(25, 1, -15);
    private RaycastHit hit;

    //Angry vars
    private float seeDistance = 9.0f;
    private Color angryColor = Color.red;
    public float followSpeed = 6.0f; //changed in inspector per level
    public AudioClip alertAudio; //assigned in editor

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        patrolLocations = PatrolLocations.GetLevelLocations(Application.loadedLevel);
        if (Application.loadedLevel != 1) { //quick way to begin patrol right away on levels 2+. a boolean check would be more re-usable
            audio.Play();
            InvokeRepeating("CheckPath", 0.1f, 0.5f);
        }
        renderer.material.color = Color.cyan;
	}

    void Update() {
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit, seeDistance)) {
            if (hit.transform.tag == "Player") {
                agent.Stop();
                if (PlayerCaught != null) {
                    PlayerCaught(transform);
                }
            }
        }
    }

    void OnEnable() {
        TriggerStartSentry.StartSentry += BeginPatrol;
        PickupKey.KeyPickedUp += BeginFollowPlayer;
    }

    void OnDisable() {
        PlayerPositionManager.PositionChanged -= MarkPlayerPosition;
        TriggerStartSentry.StartSentry -= BeginPatrol;
        PickupKey.KeyPickedUp -= BeginFollowPlayer;
    }

    void MarkPlayerPosition(Vector3 playerPos) {
        if (allowPlayerFollow) {
            agent.SetDestination(playerPos);
        }
    }

    void BeginPatrol() {
        agent.SetDestination(tutorialLocation);
        audio.Play();
        InvokeRepeating("CheckPath", 1f, 0.5f); //starts 1 sec later to ensure sentry is on path before calling check (so doesn't skip tutorial)
    }

    void BeginFollowPlayer() {
        PlayerPositionManager.PositionChanged += MarkPlayerPosition;
        CancelInvoke("CheckPath");
        agent.speed = followSpeed;
        StartCoroutine(SwapAudio());
        renderer.material.color = angryColor;
        agent.autoBraking = true;
        agent.stoppingDistance = 6.25f;
    }

    IEnumerator SwapAudio() {
        AudioClip defaultAudio = audio.clip;
        audio.maxDistance = 150f;
        audio.clip = alertAudio;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        //change clip back
        audio.clip = defaultAudio;
        audio.maxDistance = 55f;
        audio.pitch = 1.7f;
        audio.Play();
    }

    void choosePatrolLocation() {
        int randomChoice = Random.Range(0, patrolLocations.Length);
        if (randomChoice != prevPatrolLocation) {
            agent.SetDestination(patrolLocations[randomChoice]);
            prevPatrolLocation = randomChoice;
        }   //on false, will wait until next call of CheckPath() to choose again. pauses and may look weird to an observer if choice is same as prev for serveral loops. but how fix?     
    }

    /*
     * periodically check if has reached previous destination by checking if on a path. If not, choose new destination
     */
    void CheckPath() {
        if (!agent.hasPath) {
            choosePatrolLocation();
        }
    }

    /* NOT RELIABLE. JUST USE KINEMATIC (OR NO RIGIDBODY AT ALL) SENTRY. THE SENTRY WON'T GET OFF PATH SO CAN CHECK IF HAS PATH *
     * Physics moving the sentry causes path problems near destination.
     * This could be fixed by making kinematic on rigidbody (or freezing position), but this allows the sentry to clip through the player which is undesirable.
     * This method fixes that by allowing path changes when only "near" destination.
     * Best results when using small stopping distances. If stopping distance is zero, a value of 1 is set and used used.
     *
    bool DestinationReached() {
        if (agent.stoppingDistance < 1f) {
            agent.stoppingDistance = 1f;
        }
        if (agent.remainingDistance != Mathf.Infinity && agent.remainingDistance <= agent.stoppingDistance) {
            return true;
        } else return false;
    }*/
}
