using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    // Public properties
    public GameObject boidPrefab;
    public int flockSize = 20;

    [Header("Swarm Parameteres")]
    public float minVelocity = 5;
    public float maxVelocity = 20;

    public float randomness  = 1;
    public float cohesion    = 1;
    public float alignment   = 1;
    public float attraction  = 1;
    public float repulsion   = 1;
    public float range = 10;

    [Header("Swarm attractors/repulsors")]
    public GameObject[] attractors;
    public GameObject[] repulsors;

    [HideInInspector]
    public Vector3 flockCenter;
    [HideInInspector]
    public Vector3 flockVelocity;

    // Private properties
    private List<GameObject> boids;

    void Awake()
    {
        boids = new List<GameObject>();
        for (var i = 0; i < flockSize; i++)
        {
            Vector3 position = new Vector3(
                Random.value * GetComponent<Collider>().bounds.size.x,
                Random.value * GetComponent<Collider>().bounds.size.y,
                Random.value * GetComponent<Collider>().bounds.size.z
            ) - GetComponent<Collider>().bounds.extents;

            GameObject boid = Instantiate(boidPrefab, transform.position, transform.rotation) as GameObject;
            boid.transform.parent = transform;
            boid.transform.localPosition = position;
            boid.GetComponent<BoidFlocking>().SetController(this);
            boids.Add(boid);
        }
        GetComponent<Collider>().enabled = false;

        UpdateAggregateMovement();
    }
    private void Start()
    {
        foreach (GameObject boid in boids)
        {
            boid.GetComponent<BoidFlocking>().startMovment();
        }
    }

    void Update()
    {
        UpdateAggregateMovement();
    }

    void UpdateAggregateMovement()
    {
        Vector3 theCenter   = Vector3.zero;
        Vector3 theVelocity = Vector3.zero;

        foreach (GameObject boid in boids)
        {
            theCenter = theCenter + boid.transform.position;
            theVelocity = theVelocity + boid.GetComponent<Rigidbody>().velocity;
        }

        flockCenter = theCenter / (flockSize);
        flockVelocity = theVelocity / (flockSize);
    }
}