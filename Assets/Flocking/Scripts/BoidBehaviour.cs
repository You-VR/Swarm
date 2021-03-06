﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VonderBoid
{
    [Serializable]
    public class BoidBehaviour : ScriptableObject
    {

        //*************************************************************************************************************************//
        //       PUBLIC  PROPERTIES                   //
        //********************************************//
        [SerializeField]
        public string behaviourName;

        // FLOCKING
        public float minVelocity = 0.2f;
        public float maxVelocity = 3.0f;
        public float randomness = 2;
        public Vector3 maxRandomRotation = new Vector3(30.0f, 40.0f, 10.0f);

        // APPEARANCE
        public float scale = 1.0f;
        public float intensity = 1.0f;

        public BoidMovementCollection boidMovementCollection;

        private void OnEnable()
        {
            boidMovementCollection = ScriptableObject.CreateInstance<BoidMovementCollection>();
        }
    }

    [CustomPropertyDrawer(typeof(BoidBehaviour))]
    public class BoidBehaviourDrawer : PropertyDrawer
    {
        bool showBoidBehaviourTypes;

        private static GUIContent
            moveButtonContent = new GUIContent("\u21b4", "Add"),
            clearButtonContent = new GUIContent("x", "Clear");


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedObject propObj = new SerializedObject(property.objectReferenceValue);
            EditorGUILayout.PropertyField(propObj.FindProperty("behaviourName"));

            EditorGUILayout.PropertyField(propObj.FindProperty("minVelocity"));
            EditorGUILayout.PropertyField(propObj.FindProperty("maxVelocity"));
            EditorGUILayout.PropertyField(propObj.FindProperty("randomness"));
            EditorGUILayout.PropertyField(propObj.FindProperty("maxRandomRotation"));

            // Audio Settings
            showBoidBehaviourTypes = EditorGUILayout.Foldout(showBoidBehaviourTypes, "Boid Movement Types", EditorStyles.foldout);
            if (showBoidBehaviourTypes)
            {
                EditorGUILayout.PropertyField(propObj.FindProperty("boidMovementCollection"));
            }

            EditorGUI.EndProperty();

        }

        private void ShowBoidBehaviours(SerializedProperty list)
        {
            EditorGUI.indentLevel = 2;
            ShowButtons(list);

            for (int i = 0; i < list.arraySize; i++)
            {
                var element = list.GetArrayElementAtIndex(i);

                EditorGUILayout.PropertyField(element, true);
            }
        }

        private void ShowButtons(SerializedProperty list)
        {
            Debug.Log(list.arraySize);

            //selectedType = (BoidBehaviorTypes)EditorGUILayout.EnumPopup("Behaviour to create:", selectedType);
            //if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft))
            //{
            //    list.InsertArrayElementAtIndex(list.arraySize);

            //    SerializedProperty newProperty = list.GetEndProperty();

            //}
            //if (GUILayout.Button(clearButtonContent, EditorStyles.miniButtonLeft))
            //{
            //    list.ClearArray();
            //}

        }
    }
}