using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VonderBoid
{


    public class BoidController : MonoBehaviour
    {
        //*************************************************************************************************************************//
        //      PUBLIC EDITOR PROPERTIES              //
        //********************************************//

        public GameObject leftChannel;
        public GameObject rightChannel;
        
        public GameObject defaultPrefab;
        public int flockSize = 20;

        public BoidBehaviour currentBoidBehaviour { get { return boidBehaviours[0]; } }

        public List<BoidBehaviour> boidBehaviours { get { return boidBehaviourCollection.boidBehaviorList; } }
        public BoidBehaviourCollection boidBehaviourCollection;

        //*************************************************************************************************************************//
        //       OTHER   PROPERTIES                   //
        //********************************************//


        public Vector3 flockCenter   { get; private set; }
        public Vector3 flockVelocity { get; private set; }
        public Vector3 flockSTD      { get; private set; }

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
        }

        void Start()
        {
            InstantiateFlock();  // Intantiate flock

            foreach (GameObject boid in boids)
            {
                boid.GetComponent<BoidFlocking>().startMovement();
            }
        }

        void Update()
        {
            UpdateAggregateMovement();

            leftChannel.transform.position = flockCenter + flockSTD;
            rightChannel.transform.position = flockCenter - flockSTD;


            Debug.Log("!" + boidBehaviours.Count.ToString());
        }

        private void InstantiateFlock()
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

    [CustomEditor(typeof(BoidController))]
    public class BoidControllerEditor : Editor
    {
        public BoidBehaviourCollection _boidBehaviourCollection;
        SerializedObject so_boidBehaviourCollection;

        private bool 
            showGeneralSettings = true,
            showAudioOptions = false,
            showBoidBehaviours = false;

        private static GUIContent
            moveButtonContent = new GUIContent("\u21b4", "move down"),
            duplicateButtonContent = new GUIContent("+", "duplicate"),
            deleteButtonContent = new GUIContent("-", "delete");

        void OnEnable()
        {
            _boidBehaviourCollection = AssetDatabase.LoadAssetAtPath("Assets/BoidBehaviours.asset",
                             typeof(BoidBehaviourCollection)) as BoidBehaviourCollection;

            if( _boidBehaviourCollection == null)
            {
                _boidBehaviourCollection = ScriptableObject.CreateInstance<BoidBehaviourCollection>();
                _boidBehaviourCollection.boidBehaviorList = new List<BoidBehaviour>();
                AssetDatabase.CreateAsset(_boidBehaviourCollection, "Assets/BoidBehaviours.asset");
                AssetDatabase.SaveAssets();
            }

            so_boidBehaviourCollection = new UnityEditor.SerializedObject(_boidBehaviourCollection);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General Settings
            showGeneralSettings = EditorGUILayout.Foldout(showGeneralSettings, "General Settings", EditorStyles.foldout);
            if (showGeneralSettings)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultPrefab"));
                EditorGUILayout.IntSlider(serializedObject.FindProperty("flockSize"), 1, 300);
            }

            // Audio Settings
            showAudioOptions = EditorGUILayout.Foldout(showAudioOptions, "Audio Settings", EditorStyles.foldout);
            if (showAudioOptions)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("leftChannel"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rightChannel"));
            }

            showBoidBehaviours = EditorGUILayout.Foldout(showBoidBehaviours, "Boid Behaviours", EditorStyles.foldout);
            if (showBoidBehaviours)
            {
                if (GUILayout.Button("Add Boid Behaviour"))
                {
                    BoidBehaviour newBoidBehaviour = ScriptableObject.CreateInstance<BoidBehaviour>();
                    _boidBehaviourCollection.boidBehaviorList.Add(newBoidBehaviour);
                    AssetDatabase.AddObjectToAsset(newBoidBehaviour, _boidBehaviourCollection);
                    AssetDatabase.SaveAssets();
                }

                EditorGUI.indentLevel = 1;
                SerializedProperty boidBehaviourList = so_boidBehaviourCollection.FindProperty("boidBehaviorList");

                for (int i = 0; i < boidBehaviourList.arraySize; i++)
                {
                    var element = boidBehaviourList.GetArrayElementAtIndex(i);

                    EditorGUILayout.PropertyField(element, true);
                }
                serializedObject.FindProperty("boidBehaviourCollection").objectReferenceValue = _boidBehaviourCollection;
            }

            serializedObject.ApplyModifiedProperties();
        }

        //private void ShowBoidBehaviours()
        //{
        //    EditorGUI.indentLevel = 1;
        //    for (int i = 0; i < list.arraySize; i++)
        //    {
        //        var element = list.GetArrayElementAtIndex(i);

        //        EditorGUILayout.PropertyField(element, true);

        //        ShowButtons(list, i);
        //    }
        //}

        //private static void ShowButtons(SerializedProperty list, int index)
        //{
        //    if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft))
        //    {
        //        list.MoveArrayElement(index, index + 1);
        //    }
        //    if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid))
        //    {
        //        list.InsertArrayElementAtIndex(index);
        //    }
        //    if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight))
        //    {
        //        list.DeleteArrayElementAtIndex(index);
        //    }
        //}
    }
}