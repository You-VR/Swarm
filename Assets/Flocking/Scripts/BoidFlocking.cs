﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoidFlocking : MonoBehaviour
{
    enum BoidBehaviour
    {
        NORMAL,
        EXCITED,
        FREE
    };

    // Global parameters
    private float minVelocity { get { return boidController.minVelocity;   } }
    private float maxVelocity { get { return boidController.maxVelocity;   } }
    private float randomness  { get { return boidController.randomness;    } }
    private float cohesion    {
        get {
            return boidController.cohesion + (boidController.cohesion * Mathf.Sin(Time.time) / 2);
        }
    }
    private float alignment   { get { return boidController.alignment;     } }
    private float attraction  { get { return boidController.attraction; } }
    private float repulsion   { get { return boidController.repulsion;  } }
    private float cohesionRange       { get { return boidController.cohesionRange; } }
    private float   interactionRange { get { return boidController.interactionRange; } }
    private Vector3 maxRandomRotation { get { return boidController.maxRandomRotation; } }

    private GameObject[] global_attractors {
        get {
            GameObject[] array = new GameObject[boidController.attractors.Values.Count];
            boidController.attractors.Values.CopyTo(array, 0);
            return array;
        }
    }

    private GameObject[] global_repulsors  {
        get {
            GameObject[] array = new GameObject[boidController.repulsors.Values.Count];
            boidController.repulsors.Values.CopyTo(array, 0);
            return array;
        }
    }

    private Vector3 flockCenter       { get { return boidController.flockCenter;   } }
    private Vector3 flockVelocity     { get { return boidController.flockVelocity; } }

    // Local properties
    private BoidController boidController;
    private BoidBehaviour boidBehaviour;

    private GameObject[] local_attractors;
    private GameObject[] local_repulsors;




    public void startMovment()
    {
        StartCoroutine("BoidSteering");
    }

    IEnumerator BoidSteering()
    {
        while (true)
        {
            Vector3 target = getTargetVector();
            target = randomPerturbations(target);

            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + target * Time.deltaTime;

            // Enforce minimum and maximum speeds for the boids
            float speed = GetComponent<Rigidbody>().velocity.magnitude;
            if (speed > maxVelocity)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxVelocity;
            }
            else if (speed < minVelocity)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * minVelocity;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 bounce = (transform.position - collision.gameObject.transform.position);
        GetComponent<Rigidbody>().velocity = 2 * bounce * Time.deltaTime;
    }

    private Vector3 randomPerturbations( Vector3 target)
    {
        Vector3 perlin = new Vector3(   Mathf.PerlinNoise( Time.time, 1f ) * randomness,
                                        Mathf.PerlinNoise( Time.time, 2f)  * randomness,
                                        Mathf.PerlinNoise( Time.time, 3f)  * randomness);

        perlin = (perlin * 2 - new Vector3(1.0f, 1.0f, 1.0f)) * randomness;

        Quaternion randomRotation = Quaternion.Euler(Vector3.Scale(perlin, maxRandomRotation));

        float randomMagnitude = (Mathf.PerlinNoise(Time.time, 4f) * 2.0f);

        return randomRotation * target * randomMagnitude;
    }

    private Vector3 getTargetVector()
    {
        return      cohesion   * getCohesionVector()  +
                    alignment  * getAlignmentVector() +
                    repulsion  * getRepulsionVector() +
                    attraction * getAttractionVector();
    }

    private Vector3 getCohesionVector()
    {
        return tickFunction((flockCenter - transform.position), cohesionRange);
    }
    private Vector3 getAlignmentVector()
    {
        return (flockVelocity - GetComponent<Rigidbody>().velocity) * Time.deltaTime;
    }

    private Vector3 getRepulsionVector()
    {
        Vector3 repulsionVector = Vector3.zero;
        foreach (GameObject repuslor in global_repulsors)
        {
            if (repuslor != null) { repulsionVector += tickFunction(transform.position - repuslor.transform.position, interactionRange); }
        }
        return repulsionVector;
    }

    private Vector3 getAttractionVector()
    {
        Vector3 attractionVector = Vector3.zero;
        foreach (GameObject attractor in global_attractors)
        {
            if (attractor != null) { attractionVector += tickFunction(attractor.transform.position - transform.position, interactionRange); }
        }
        return attractionVector;
    }

    public void SetController(BoidController theBoidController)
    {
        boidController = theBoidController;

        boidBehaviour = BoidBehaviour.NORMAL;
    }


    //********************************************//
    //       MATHS FUNCTIONS                      //
    //********************************************//

    private float tickFunction(float x, float p)
    {
        float sign = Mathf.Sign(x);
        x = Mathf.Abs(x);

        x = x - p;

        return x * sign;


    }
    private Vector3 tickFunction(Vector3 x, float range)
    {
        return x.normalized * tickFunction(x.magnitude, range);
    }
}