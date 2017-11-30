using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace VonderBoid
{
    public class BoidMovementType
    {
        public string behaviourName { get; protected set; }

        public float intensity { get; set; }

        public void OnGUI() {
            EditorGUILayout.LabelField(behaviourName);
            intensity = EditorGUILayout.Slider("intensity", intensity, 0, 10); 
        }

    public virtual Vector3 getTargetVector(BoidFlocking boid) {
            return Vector3.zero;
        }


        //********************************************//
        //       MATHS FUNCTIONS                      //
        //********************************************//

        protected static float tickFunction(float x, float p)
        {
            float sign = Mathf.Sign(x);
            x = Mathf.Abs(x);

            x = x - p;

            return x * sign;


        }
        protected static Vector3 tickFunction(Vector3 x, float range)
        {
            return x.normalized * tickFunction(x.magnitude, range);
        }
    }

    public class BoidMovementTypeCohesion : BoidMovementType
    {
        public float cohesionRange = 2.0f;

        public BoidMovementTypeCohesion()
        {
            behaviourName = "Cohesion";
        }

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            return tickFunction((boid.flockCenter - boid.transform.position), cohesionRange);
        }
    }

    public class BoidMovementTypeAlignment : BoidMovementType
    {     
        public BoidMovementTypeAlignment()
        {
            behaviourName = "Alignment";
        }

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            return (boid.flockVelocity - boid.velocity) * Time.deltaTime;
        }
    }

    public class BoidMovementTypeOrbit : BoidMovementType
    {
        public List<GameObject> targets;

        public BoidMovementTypeOrbit()
        {
            behaviourName = "Orbit";
        }

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            Vector3 attractionVector = Vector3.zero;
            if (targets.Count > 0)
            {
                Vector3 targetsPos = targets[0].transform.position;
                return Vector3.Cross(targetsPos - boid.transform.position, Vector3.up);
            }
            else
            {
                return attractionVector;
            }
        }
    }

    public class BoidMovementTypeRepulsion : BoidMovementType
    {
        public List<GameObject> targets;
        float interactionRange = 2.0f;

        public BoidMovementTypeRepulsion()
        {
            behaviourName = "Repulsion";
        }

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            Vector3 repulsionVector = Vector3.zero;
            if (targets.Count > 0)
            {
                foreach (GameObject repuslor in targets)
                {
                    if (repuslor != null) { repulsionVector += tickFunction(boid.transform.position - repuslor.transform.position, interactionRange); }
                }
                return repulsionVector / targets.Count;
            }
            else
            {
                return repulsionVector;
            }
        }
    }

    public class BoidMovementTypeAttraction : BoidMovementType
    {
        public List<GameObject> targets;
        float interactionRange = 2.0f;

        public BoidMovementTypeAttraction()
        {
            behaviourName = "Attraction";
        }

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            Vector3 attractionVector = Vector3.zero;
            if (targets.Count > 0)
            {
                foreach (GameObject attractor in targets)
                {
                    if (attractor != null) { attractionVector += tickFunction(attractor.transform.position - boid.transform.position, interactionRange); }
                }
                return attractionVector / targets.Count;
            }
            else
            {
                return attractionVector;
            }
        }
    }


    //[CustomPropertyDrawer(typeof(BoidMovementType))]
    //public class BoidBehaviourTypeDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        Debug.Log("Here");
    //        EditorGUILayout.PropertyField(property, true);
    //    }
    //}
}