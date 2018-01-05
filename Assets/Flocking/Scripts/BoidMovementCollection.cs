using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VonderBoid
{
    [Serializable]
    public class BoidMovementCollection : ScriptableObject
    {
        enum BoidMovementTypes
        {
            Attraction,
            Repulsion
        }

        [SerializeField]
        public List<BoidMovementType> boidMovementTypes;

        private void OnEnable()
        {
            boidMovementTypes = new List<BoidMovementType>();
            boidMovementTypes.Add(new BoidMovementTypeAttraction());
        }

    }


    [CustomPropertyDrawer(typeof(BoidMovementCollection))]
    public class BoidMovementCollectionDrawer : PropertyDrawer
    {
        SerializedProperty sp_boidMovementTypes;

        BoidMovementType.BoidBehaviorTypes selectedType = BoidMovementType.BoidBehaviorTypes.Alignment;

        private static GUIContent
            moveButtonContent = new GUIContent("\u21b4", "Add"),
            clearButtonContent = new GUIContent("x", "Clear");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedObject propObj = new SerializedObject(property.objectReferenceValue);

            propObj.Update();

            sp_boidMovementTypes = propObj.FindProperty("boidMovementTypes");

            if(sp_boidMovementTypes != null)
            {
                ShowBoidMovementTypes();
            }

            propObj.ApplyModifiedProperties();


        }

        private void ShowBoidMovementTypes()
        {
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            ShowButtons();

            for (int i = 0; i < sp_boidMovementTypes.arraySize; i++)
            {
                var element = sp_boidMovementTypes.GetArrayElementAtIndex(i);

                EditorGUILayout.PropertyField(element, true);
            }
        }


        private void ShowButtons()
        {
            
            selectedType = (BoidMovementType.BoidBehaviorTypes)EditorGUILayout.EnumPopup("Behaviour to create:", selectedType);

            if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft))
            {
                sp_boidMovementTypes.InsertArrayElementAtIndex(sp_boidMovementTypes.arraySize);

                SerializedProperty newProperty = sp_boidMovementTypes.GetArrayElementAtIndex(sp_boidMovementTypes.arraySize - 1);

                BoidMovementType newBoidMovementType = BoidMovementType.CreateMovementType(selectedType);

                newProperty.objectReferenceValue = newBoidMovementType;

            }
            if (GUILayout.Button(clearButtonContent, EditorStyles.miniButtonLeft))
            {
                sp_boidMovementTypes.ClearArray();
            }

        }
    }
}
