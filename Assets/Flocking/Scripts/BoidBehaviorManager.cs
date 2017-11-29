//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//namespace VonderBoid
//{
//    public class BoidBehaviorManager : MonoBehaviour
//    {

//        [Header("Boid Controller")]
//        public BoidController boidController;

//        [Header("Game objects")]
//        public GameObject leftHand;
//        public GameObject rightHand;
//        public GameObject head;
//        public GameObject glowSphere;
//        public GameObject flower;


//        private Dictionary<string, BoidBehaviour> stateList = new Dictionary<string, BoidBehaviour>();

//        void Awake()
//        {
//            BoidBehaviour boidBehaviour = new BoidBehaviour();
//            //*******************************************************
//            // START BEHAVIOUR

//            boidBehaviour.minVelocity = 0.2f;
//            boidBehaviour.maxVelocity = 1.5f;
//            boidBehaviour.randomness = 4.0f;
//            boidBehaviour.cohesion = 0.5f;
//            boidBehaviour.alignment = -1.0f;
//            boidBehaviour.attraction = 1.0f;
//            boidBehaviour.repulsion = 2.2f;
//            boidBehaviour.cohesionRange = 10.0f;
//            boidBehaviour.interactionRange = 3.0f;

//            boidBehaviour.setDefaultAttractors(new GameObject[] {  glowSphere,
//                                                                flower      });

//            boidBehaviour.setDefaultRepulsors(new GameObject[] {  leftHand,
//                                                                rightHand,
//                                                                head        });

//            boidBehaviour.scale = 1.0f;
//            boidBehaviour.intensity = 1.0f;

//            stateList.Add("Start", boidBehaviour);

//            //*******************************************************
//            // BALL TOUCHED
//            boidBehaviour = new BoidBehaviour();

//            boidBehaviour.minVelocity = 0.2f;
//            boidBehaviour.maxVelocity = 2.5f;
//            boidBehaviour.randomness = 4.0f;
//            boidBehaviour.cohesion = 0.5f;
//            boidBehaviour.alignment = -1.0f;
//            boidBehaviour.attraction = 1.0f;
//            boidBehaviour.repulsion = 2.2f;
//            boidBehaviour.cohesionRange = 4.0f;
//            boidBehaviour.interactionRange = 2.0f;
//            boidBehaviour.setDefaultAttractors(new GameObject[] {  glowSphere,
//                                                                flower      });

//            boidBehaviour.setDefaultRepulsors(new GameObject[] {  leftHand,
//                                                                rightHand,
//                                                                head        });

//            boidBehaviour.scale = 1.0f;
//            boidBehaviour.intensity = 0.5f;

//            stateList.Add("Ball_touched", boidBehaviour);

//            //*******************************************************
//            // BALL ON POLE
//            boidBehaviour = new BoidBehaviour();
//            boidBehaviour.minVelocity = 1.0f;
//            boidBehaviour.maxVelocity = 3.0f;
//            boidBehaviour.randomness = 2.0f;
//            boidBehaviour.cohesion = 1.0f;
//            boidBehaviour.alignment = 1.0f;
//            boidBehaviour.attraction = 1.0f;
//            boidBehaviour.repulsion = 1.2f;
//            boidBehaviour.cohesionRange = 2.0f;
//            boidBehaviour.interactionRange = 1.0f;

//            boidBehaviour.setDefaultAttractors(new GameObject[] {  glowSphere,
//                                                                flower      });

//            boidBehaviour.setDefaultRepulsors(new GameObject[] {  leftHand,
//                                                                rightHand,
//                                                                head        });

//            boidBehaviour.scale = 2.0f;
//            boidBehaviour.intensity = 2.0f;

//            stateList.Add("Ball_on_pole", boidBehaviour);

//            //*******************************************************
//            // FLOWER IN HAND
//            boidBehaviour = new BoidBehaviour();

//            boidBehaviour.minVelocity = 0.2f;
//            boidBehaviour.maxVelocity = 2.5f;
//            boidBehaviour.randomness = 1.0f;
//            boidBehaviour.cohesion = 1.5f;
//            boidBehaviour.alignment = 1.0f;
//            boidBehaviour.attraction = 3.0f;
//            boidBehaviour.repulsion = 2.2f;
//            boidBehaviour.cohesionRange = 0.5f;
//            boidBehaviour.interactionRange = 0.5f;

//            boidBehaviour.setDefaultAttractors(new GameObject[] {   leftHand,
//                                                                rightHand,
//                                                                head,
//                                                                flower      });

//            boidBehaviour.setDefaultRepulsors(new GameObject[] { });

//            boidBehaviour.scale = 0.7f;
//            boidBehaviour.intensity = 1.0f;

//            stateList.Add("Flower_in_hand", boidBehaviour);

//        }

//        public void changeState(string name)
//        {
//            if (stateList.ContainsKey(name))
//            {
//                boidController.currentBoidBehaviour = stateList[name];
//            }
//            else
//            {
//                Debug.LogWarning(name + " not found in predefined states.");
//            }

//        }
//        void Start()
//        {
//            StartCoroutine("cycleStates");
//        }

//        IEnumerator cycleStates()
//        {
//            int i = 0;
//            string[] temp = new string[stateList.Count];
//            stateList.Keys.CopyTo(temp, 0);

//            while (true)
//            {
//                Debug.Log("State change to " + temp[i]);

//                boidController.currentBoidBehaviour = stateList[temp[i]];

//                i++;
//                if (i == stateList.Count) { i = 0; }
//                yield return new WaitForSeconds(20.0f);
//            }
//        }
//    }
//}
