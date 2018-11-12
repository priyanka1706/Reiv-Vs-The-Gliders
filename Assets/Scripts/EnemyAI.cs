using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class EnemyAI : MonoBehaviour {

    // what to chase
    public Transform target;

    // how many times a second we will updateour path
    public float updateRate = 2f;

    // caching
    private Seeker seeker;
    private Rigidbody2D rb;

    // the calculated path
    public Path path;

    // the AI's speed per sec
    public float speed = 200f;
    public ForceMode2D fMode;
    
    [HideInInspector]
    public bool pathIsEnded = false;

    // the max dist from the AI to a way pt for it to continue to the next waypoint
    public float nextWayPointDistance = 3;

    // the waypoint we are currently moving towards
    private int currentWayPoint = 0;

    private bool searchingForPlayer = false;

    void Start () {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null) { 
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        // start a new path to the target pos, return the result to the OnPathCmplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator SearchForPlayer() {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null) {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        } else {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }

    IEnumerator UpdatePath() {
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p) {
        Debug.LogError("path had an error " + p.error);
        if(!p.error) {
            path = p;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate () {

        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        if (path == null)
            return;

        if(currentWayPoint >= path.vectorPath.Count) {
            if (pathIsEnded)
                return;

            Debug.LogError("End of path reached");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        // direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //  move the AI
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
        if (dist < nextWayPointDistance) {
            currentWayPoint++;
            return;
        }
    }
}
