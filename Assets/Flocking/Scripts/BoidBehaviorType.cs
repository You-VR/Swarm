﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace VonderBoid
{

    [Serializable]
    public class BoidBehaviorType : ScriptableObject
    {
        public virtual string behaviourName { get; private set; }

        [SerializeField]
        public float intensity = 1.0f;

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

    public class BoidBehaviorCohesion : BoidBehaviorType
    {
        public new string behaviourName = "Cohesion";
        public float cohesionRange = 2.0f;

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            return tickFunction((boid.flockCenter - boid.transform.position), cohesionRange);
        }
    }

    public class BoidBehaviorAlignment : BoidBehaviorType
    {
        public new string behaviourName = "Alignment";

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            return (boid.flockVelocity - boid.velocity) * Time.deltaTime;
        }
    }

    public class BoidBehaviorOrbit : BoidBehaviorType
    {
        public new string behaviourName = "Orbit";
        public List<GameObject> targets;

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

    public class BoidBehaviorRepulsion : BoidBehaviorType
    {
        public new string behaviourName = "Repulsion";
        public List<GameObject> targets;
        float interactionRange = 2.0f;

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

    public class BoidBehaviorAttraction : BoidBehaviorType
    {
        public new string behaviourName = "Attraction";
        public List<GameObject> targets;
        float interactionRange = 2.0f;

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


    [CustomPropertyDrawer(typeof(BoidBehaviorType))]
    public class BoidBehaviourTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property, true);
        }
    }
}