using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAppearance : MonoBehaviour {
    // Global parameters
    private float scale      { get { return boidController.boidBehaviour.scale; } }
    private float intensity { get { return boidController.boidBehaviour.intensity; } }


    // Local properties
    private BoidController boidController;

    Light boidLight;

    // Use this for initialization
    void Start () {
        boidLight = GetComponent<Light>();
        if (boidLight == null) { boidLight = this.gameObject.AddComponent<Light>(); }

        boidLight.intensity = intensity;
        boidLight.range = 10.0f;
        boidLight.type = LightType.Point;

	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localScale = new Vector3 (scale, scale, scale);
	}

    public void SetController(BoidController theBoidController)
    {
        boidController = theBoidController;
    }


}
