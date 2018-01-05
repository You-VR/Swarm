using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace VonderBoid
{
    
    public class BoidMovementType : ScriptableObject
    {
        public enum BoidBehaviorTypes
        {
            Cohesion,
            Alignment,
            Orbit,
            Repulsion,
            Attraction
        }

        public virtual string behaviourName { get { return "Default"; } }
        public float intensity;

        public void OnGUI() {
            EditorGUILayout.LabelField(behaviourName);
            intensity = EditorGUILayout.Slider("intensity", intensity, 0, 10); 
        }

        public virtual Vector3 getTargetVector(BoidFlocking boid) {
            return Vector3.zero;
        }

        public static BoidMovementType CreateMovementType(BoidBehaviorTypes boidBehaviourType)
        {
            switch (boidBehaviourType)
            {
                case BoidBehaviorTypes.Cohesion:
                    return new BoidMovementTypeCohesion();
                case BoidBehaviorTypes.Alignment:
                    return new BoidMovementTypeAlignment();
                case BoidBehaviorTypes.Orbit:
                    return new BoidMovementTypeOrbit();
                case BoidBehaviorTypes.Repulsion:
                    return new BoidMovementTypeRepulsion();
                case BoidBehaviorTypes.Attraction:
                    return new BoidMovementTypeAttraction();
                default:
                    return new BoidMovementType();

            }
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
    [CustomPropertyDrawer(typeof(BoidMovementType))]
    public class BoidMovementTypeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedObject propObj = new SerializedObject(property.objectReferenceValue);
            propObj.Update();
            EditorGUILayout.LabelField(((BoidMovementType)propObj.targetObject).behaviourName);
            EditorGUILayout.Slider(propObj.FindProperty("intensity"), 0.0f, 5.0f);
            propObj.ApplyModifiedProperties();
        }
    }

    
    public class BoidMovementTypeCohesion : BoidMovementType
    {
        public float cohesionRange = 2.0f;
        public override string behaviourName { get { return "Cohesion"; } }

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            return tickFunction((boid.flockCenter - boid.transform.position), cohesionRange);
        }
    }
    [CustomPropertyDrawer(typeof(BoidMovementTypeCohesion))]
    public class BoidMovementTypeCohesionEditor : BoidMovementTypeEditor
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
    }


    public class BoidMovementTypeAlignment : BoidMovementType
    {
        public override string behaviourName { get { return "Alignment"; } }

        public override Vector3 getTargetVector(BoidFlocking boid)
        {
            return (boid.flockVelocity - boid.velocity) * Time.deltaTime;
        }
    }
    [CustomPropertyDrawer(typeof(BoidMovementTypeAlignment))]
    public class BoidMovementTypeAlignmentEditor : BoidMovementTypeEditor
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
    }

    public class BoidMovementTypeOrbit : BoidMovementType
    {
        public List<GameObject> targets;
        public override string behaviourName { get { return "Orbit"; } }

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
    [CustomPropertyDrawer(typeof(BoidMovementTypeOrbit))]
    public class BoidMovementTypeOrbitEditor : BoidMovementTypeEditor
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
    }

    public class BoidMovementTypeRepulsion : BoidMovementType
    {
        public List<GameObject> targets;
        float interactionRange = 2.0f;

        public override string behaviourName { get { return "Repulsion"; } }

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
    [CustomPropertyDrawer(typeof(BoidMovementTypeRepulsion))]
    public class BoidMovementTypeRepulsionEditor : BoidMovementTypeEditor
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
    }

    public class BoidMovementTypeAttraction : BoidMovementType
    {
        public List<GameObject> targets;
        float interactionRange = 2.0f;

        public override string behaviourName { get { return "Attraction"; } }
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
    [CustomPropertyDrawer(typeof(BoidMovementTypeAttraction))]
    public class BoidMovementTypeAttractionEditor : BoidMovementTypeEditor
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
    }
}