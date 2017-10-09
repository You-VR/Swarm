using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoidFlocking : MonoBehaviour
{
    private BoidController boidController;


    private bool inited = false;
    private float minVelocity { get { return boidController.minVelocity;   } }
    private float maxVelocity { get { return boidController.maxVelocity;   } }
    private float randomness  { get { return boidController.randomness;    } }
    Vector3 flockCenter       { get { return boidController.flockCenter;   } }
    Vector3 flockVelocity     { get { return boidController.flockVelocity; } }

    void Start()
    {
        StartCoroutine("BoidSteering");
    }

    IEnumerator BoidSteering()
    {
        while (true)
        {
            if (inited)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity + Calc() * Time.deltaTime;

                // enforce minimum and maximum speeds for the boids
                float speed = GetComponent<Rigidbody>().velocity.magnitude;
                if (speed > maxVelocity)
                {
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxVelocity;
                }
                else if (speed < minVelocity)
                {
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * minVelocity;
                }
            }

            float waitTime = Random.Range(0.3f, 0.5f);
            yield return new WaitForSeconds(waitTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<BoidFlocking>() != null)
        {
            GetComponent<Rigidbody>().velocity = -GetComponent<Rigidbody>().velocity;
        }

    }

    private Vector3 Calc()
    {
        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

        randomize.Normalize();

        Vector3 relFlockCenter = flockCenter - transform.localPosition;
        Vector3 relFlockVelocity = flockVelocity - GetComponent<Rigidbody>().velocity;

        return (relFlockCenter + relFlockVelocity + randomize * randomness);
    }

    public void SetController(BoidController theBoidController)
    {
        boidController = theBoidController;
        inited = true;
    }
}