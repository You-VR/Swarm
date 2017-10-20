using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviour{
    //*************************************************************************************************************************//
    //       PUBLIC  PROPERTIES                   //
    //********************************************//
    // FLOCKING
    public float minVelocity = 0.2f;
    public float maxVelocity = 3.0f;
    public float randomness = 2;
    public float cohesion = 1;
    public float alignment = 1;
    public float attraction = 1;
    public float repulsion = 1;
    public float cohesionRange = 4;
    public float interactionRange = 1;
    public Vector3 maxRandomRotation = new Vector3(30.0f, 40.0f, 10.0f);
    public Dictionary<string, GameObject> attractors = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> repulsors  = new Dictionary<string, GameObject>();

    // APPEARANCE
    public float scale = 1.0f;
    public float intensity = 1.0f;

    //*************************************************************************************************************************//
    //       PUBLIC  METHODS                      //
    //********************************************//
    public void setDefaultAttractors(GameObject[] _attractors)
    {
        attractors.Clear();
        int count = 0;
        foreach (GameObject gameObject in _attractors)
        {
            if (gameObject != null) { addAttractor(gameObject, "Default_" + count.ToString()); }
            count++;
        }
    }

    public void setDefaultRepulsors(GameObject[] _repulsors)
    {
        repulsors.Clear();
        int count = 0;
        foreach (GameObject gameObject in _repulsors)
        {
            if (gameObject != null) { addRepulsor(gameObject, "Default_" + count.ToString()); }
            count++;
        }
    }

    public int removeAttractor(GameObject gameObject, string name) { return removeFromDictionary(attractors, gameObject, name); }

    public int addAttractor(GameObject gameObject, string name) { return addToDictionary(attractors, gameObject, name); }

    public int removeRepulsor(GameObject gameObject, string name) { return removeFromDictionary(repulsors, gameObject, name); }

    public int addRepulsor(GameObject gameObject, string name) { return addToDictionary(repulsors, gameObject, name); }

    //*************************************************************************************************************************//
    //       PRIVATE METHODS                      //
    //********************************************//

    private int removeFromDictionary(Dictionary<string, GameObject> dictionary, GameObject gameObject, string name)
    {
        if (!dictionary.ContainsKey(name))
        {
            dictionary.Remove(name);
            return 0;
        }
        else
        {
            return -1;
        }
    }
    private int addToDictionary(Dictionary<string, GameObject> dictionary, GameObject gameObject, string name)
    {
        if (dictionary.ContainsKey(name))
        {
            return -1;
        }
        else
        {
            dictionary.Add(name, gameObject);
            return 0;
        }
    }
}
