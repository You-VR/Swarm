using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public List<BoidMovementType> boidBehaviorTypes;
    }
}
