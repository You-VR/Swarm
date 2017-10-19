using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    //*************************************************************************************************************************//
    //      PUBLIC EDITOR PROPERTIES              //
    //********************************************//
    public GameObject defaultPrefab;
    public int flockSize = 20;

    [Header("Swarm Parameteres")]
    public float minVelocity = 0.2f;
    public float maxVelocity = 3.0f;

    public float randomness  = 2;
    public float cohesion    = 1;
    public float alignment   = 1;
    public float attraction  = 1;
    public float repulsion   = 1;
    public float cohesionRange = 4;
    public float interactionRange = 1;
    public Vector3 maxRandomRotation = new Vector3(30.0f, 40.0f, 10.0f);

    [Header("Default swarm attractors/repulsors")]
    public GameObject[] _attractors;
    public GameObject[] _repulsors;

    //*************************************************************************************************************************//
    //       OTHER   PROPERTIES                   //
    //********************************************//

    public Dictionary<string, GameObject> attractors { get; private set; }
    public Dictionary<string, GameObject> repulsors  { get; private set; }
    public Vector3 flockCenter   { get; private set; }
    public Vector3 flockVelocity { get; private set; }

    //*************************************************************************************************************************//
    //       PRIVATE PROPERTIES                   //
    //********************************************//

    private List<GameObject> boids;

    //*************************************************************************************************************************//
    //       PUBLIC METHODS                       //
    //********************************************//

    public void swapPrefab(GameObject newBoidBody)
    {
        foreach (GameObject boid in boids)
        {
            foreach(Transform child in boid.transform)
            {
                Destroy(child.gameObject);
            }
            Instantiate(newBoidBody, boid.transform );
        }
    }

    public int removeAttractor(GameObject gameObject, string name) { return removeFromDictionary(attractors, gameObject, name); }

    public int addAttractor(   GameObject gameObject, string name) { return addToDictionary(attractors, gameObject, name); }

    public int removeRepulsor( GameObject gameObject, string name) { return removeFromDictionary(repulsors, gameObject, name); }

    public int addRepulsor(    GameObject gameObject, string name) { return addToDictionary(repulsors, gameObject, name); }



    //*************************************************************************************************************************//
    //       PRIVATE METHODS                      //
    //********************************************//

    void Awake()
    {
        boids = new List<GameObject>();
        instantiateFlock();  // Intantiate flock

        // Set default attractors
        attractors = new Dictionary<string, GameObject>();
        setDefaultInteractions(attractors, _attractors);

        // Set default repulsors
        repulsors = new Dictionary<string, GameObject>();
        setDefaultInteractions(repulsors, _repulsors);

        UpdateAggregateMovement();
    }
    void Start()
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

    private void instantiateFlock()
    {
        for (var i = 0; i < flockSize; i++)
        {
            Vector3 position = new Vector3(
                Random.value * GetComponent<Collider>().bounds.size.x,
                Random.value * GetComponent<Collider>().bounds.size.y,
                Random.value * GetComponent<Collider>().bounds.size.z
            ) - GetComponent<Collider>().bounds.extents;

            GameObject newBoid = new GameObject("Boid_" + i.ToString());

            newBoid.transform.parent = transform;
            newBoid.transform.localPosition = position;

            newBoid.AddComponent<BoidFlocking>();
            newBoid.GetComponent<BoidFlocking>().SetController(this);

            newBoid.AddComponent<Rigidbody>();
            newBoid.GetComponent<Rigidbody>().freezeRotation = true;
            newBoid.GetComponent<Rigidbody>().useGravity = false;

            Instantiate(defaultPrefab, newBoid.transform);

            boids.Add(newBoid);
        }
        // Turn of spawn collider
        GetComponent<Collider>().enabled = false;
    }

    private void setDefaultInteractions(Dictionary<string, GameObject> interactor, GameObject[] _interactors )
    {
        int count = 0;
        foreach (GameObject gameObject in _interactors)
        {
            if (gameObject != null) { interactor.Add("Default_" + count.ToString(), gameObject); }
            count++;
        }
    }

    private void UpdateAggregateMovement()
    {
        Vector3 theCenter = Vector3.zero;
        Vector3 theVelocity = Vector3.zero;

        foreach (GameObject boid in boids)
        {
            theCenter = theCenter + boid.transform.position;
            theVelocity = theVelocity + boid.GetComponent<Rigidbody>().velocity;
        }

        flockCenter = theCenter / (flockSize);
        flockVelocity = theVelocity / (flockSize);
    }

    private int addToDictionary(Dictionary<string, GameObject> dictionary, GameObject gameObject, string name)
    {
        if (dictionary.ContainsKey(name))
        {
            dictionary.Remove(name);
            return 0;
        }
        else
        {
            return -1;
        }
    }
    private int removeFromDictionary(Dictionary<string, GameObject> dictionary, GameObject gameObject, string name)
    {
        if (dictionary.ContainsKey(name))
        {
            return -1;
        }
        else
        {
            dictionary.Add(name, gameObject);
            return 0;
        }
    }

}