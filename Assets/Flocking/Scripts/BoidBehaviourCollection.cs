using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VonderBoid
{
    public class BoidBehaviourCollection : ScriptableObject
    {
        [SerializeField]
        public List<BoidBehaviour> boidBehaviorList;

        public int testInt;
    }
}