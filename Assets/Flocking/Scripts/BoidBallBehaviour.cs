using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoidController))]
public class BoidBallBehaviour : MonoBehaviour {

    private BoidController boidController { get { return GetComponent<BoidController>(); } }

    public int removeAttractor(GameObject gameObject, string name) { return removeFromDictionary(boidController.attractors, gameObject, name); }
    public int addAttractor(GameObject gameObject, string name)    { return addToDictionary(boidController.attractors, gameObject, name); }
    public int removeRepulsor(GameObject gameObject, string name)  { return removeFromDictionary(boidController.repulsors, gameObject, name); }
    public int addRepulsor(GameObject gameObject, string name)     { return addToDictionary(boidController.repulsors, gameObject, name); }

    private int addToDictionary(Dictionary<string, GameObject> dictionary, GameObject gameObject, string name)
    {
        if (dictionary.ContainsKey(name))
        {
            dictionary.Remove(name);
            return 0;
        }
        else
        {
            return -1;
        }
    }
    private int removeFromDictionary(Dictionary<string, GameObject> dictionary, GameObject gameObject, string name) { 
        if (dictionary.ContainsKey(name))
        {
            return -1;
        } else
        {
            dictionary.Add(name, gameObject);
            return 0;
        }
        
    }
}
