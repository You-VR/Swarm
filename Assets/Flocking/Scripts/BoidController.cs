using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VonderBoid
{

    public class BoidController : MonoBehaviour
    {
        //*************************************************************************************************************************//
        //      PUBLIC EDITOR PROPERTIES              //
        //********************************************//
        [Header("Audio Sources")]
        public GameObject leftChannel;
        public GameObject rightChannel;

        [Header("Init Parameters")]
        public GameObject defaultPrefab;
        public int flockSize = 20;
        public GameObject[] attractors;
        public GameObject[] repulsors;


        [Header("Swarm Parameters")]
        public float minVelocity = 0.2f;
        public float maxVelocity = 3.0f;

        public float randomness = 2;
        public float cohesion = 1;
        public float alignment = 1;
        public float attraction = 1;
        public float orbit = 1;
        public float repulsion = 1;
        public float cohesionRange = 4;
        public float interactionRange = 1;
        public Vector3 maxRandomRotation = new Vector3(30.0f, 40.0f, 10.0f);


        //*************************************************************************************************************************//
        //       OTHER   PROPERTIES                   //
        //********************************************//
        public BoidBehaviour boidBehaviour;

        public Vector3 flockCenter { get; private set; }
        public Vector3 flockVelocity { get; private set; }
        public Vector3 flockSTD { get; private set; }

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
                foreach (Transform child in boid.transform)
                {
                    Destroy(child.gameObject);
                }
                Instantiate(newBoidBody, boid.transform);
            }
        }


        //*************************************************************************************************************************//
        //       PRIVATE METHODS                      //
        //********************************************//

        void Awake()
        {
            boids = new List<GameObject>();
            instantiateFlock();  // Intantiate flock

            boidBehaviour = new BoidBehaviour();
            loadPropertiesToBoidBehaviour();

            UpdateAggregateMovement();
        }
        private void OnValidate()
        {
            loadPropertiesToBoidBehaviour();
        }
        private void loadPropertiesToBoidBehaviour()
        {
            if (boidBehaviour != null)
            {
                boidBehaviour.minVelocity = minVelocity;
                boidBehaviour.maxVelocity = maxVelocity;
                boidBehaviour.randomness = randomness;
                boidBehaviour.cohesion = cohesion;
                boidBehaviour.alignment = alignment;
                boidBehaviour.orbit = orbit;
                boidBehaviour.attraction = attraction;
                boidBehaviour.repulsion = repulsion;
                boidBehaviour.cohesionRange = cohesionRange;
                boidBehaviour.interactionRange = interactionRange;
                boidBehaviour.maxRandomRotation = maxRandomRotation;

                boidBehaviour.setDefaultAttractors(attractors);
                boidBehaviour.setDefaultRepulsors(repulsors);
            }

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

            leftChannel.transform.position = flockCenter + flockSTD;
            rightChannel.transform.position = flockCenter - flockSTD;
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

                newBoid.AddComponent<BoidAppearance>();
                newBoid.GetComponent<BoidAppearance>().SetController(this);

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


        private void UpdateAggregateMovement()
        {
            Vector3 theCenter = Vector3.zero;
            Vector3 theVelocity = Vector3.zero;
            Vector3 theSTD = Vector3.zero;

            foreach (GameObject boid in boids)
            {
                theCenter += boid.transform.position;
                theVelocity += boid.GetComponent<Rigidbody>().velocity;
            }

            flockCenter = theCenter / (flockSize);
            flockVelocity = theVelocity / (flockSize);

            foreach (GameObject boid in boids)
            {
                theSTD += new Vector3(Mathf.Pow(boid.transform.position.x - flockCenter.x, 2),
                                        Mathf.Pow(boid.transform.position.y - flockCenter.y, 2),
                                        Mathf.Pow(boid.transform.position.z - flockCenter.z, 2));
            }

            theSTD = theSTD / (flockSize - 1);
            flockSTD = new Vector3(Mathf.Sqrt(theSTD.x),
                                    Mathf.Sqrt(theSTD.y),
                                    Mathf.Sqrt(theSTD.z));

            flockCenter = theCenter / (flockSize);
            flockVelocity = theVelocity / (flockSize);
        }
    }
}