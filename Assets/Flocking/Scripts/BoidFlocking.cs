using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VonderBoid
{
    public class BoidFlocking : MonoBehaviour
    {
        private BoidController boidController;
        private BoidBehaviour boidBehaviour {  get { return boidController.currentBoidBehaviour; } }

        private float minVelocity { get { return boidBehaviour.minVelocity; } }
        private float maxVelocity { get { return boidBehaviour.maxVelocity; } }
        private float randomness  { get { return boidBehaviour.randomness; } }
        private Vector3 maxRandomRotation { get { return boidBehaviour.maxRandomRotation; } }

        public Vector3 velocity { get { return GetComponent<Rigidbody>().velocity; } private set { GetComponent<Rigidbody>().velocity = value; } }

        public Vector3 flockCenter   { get { return boidController.flockCenter; } }
        public Vector3 flockVelocity { get { return boidController.flockVelocity; } }


        public void startMovement()
        {
            StartCoroutine("BoidSteering");
        }

        IEnumerator BoidSteering()
        {
            while (true)
            {
                velocity *= 0.99f; // Make variable

                Vector3 target = getTargetVector();
                target = randomPerturbations(target);

                velocity = velocity + target * Time.deltaTime;

                // Enforce minimum and maximum speeds for the boids
                float speed = velocity.magnitude;
                if (speed > maxVelocity)
                {
                    velocity = velocity.normalized * maxVelocity;
                }
                else if (speed < minVelocity)
                {
                    velocity = velocity.normalized * minVelocity;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Vector3 bounce = (transform.position - collision.gameObject.transform.position);
            GetComponent<Rigidbody>().velocity = 2 * bounce * Time.deltaTime;
        }

        private Vector3 randomPerturbations(Vector3 target)
        {
            Vector3 perlin = new Vector3(Mathf.PerlinNoise(Time.time, 1f) * randomness,
                                            Mathf.PerlinNoise(Time.time, 2f) * randomness,
                                            Mathf.PerlinNoise(Time.time, 3f) * randomness);

            perlin = (perlin * 2 - new Vector3(1.0f, 1.0f, 1.0f)) * randomness;

            Quaternion randomRotation = Quaternion.Euler(Vector3.Scale(perlin, maxRandomRotation));

            float randomMagnitude = (Mathf.PerlinNoise(Time.time, 4f) * 2.0f);

            return randomRotation * target * randomMagnitude;
        }

        private Vector3 getTargetVector()
        {
            Vector3 targetVector = Vector3.zero;

            return targetVector;
        }


        public void SetController(BoidController theBoidController)
        {
            boidController = theBoidController;
        }
    }
}