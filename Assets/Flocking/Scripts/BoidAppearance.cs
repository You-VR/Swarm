using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAppearance : MonoBehaviour {
    // Global parameters
    private float scale      { get { return boidController.boidBehaviour.scale; } }
    private float intensity { get { return boidController.boidBehaviour.intensity; } }


    // Local properties
    private BoidController boidController;
    private float offset;

    Light boidLight;

    // Use this for initialization
    void Start () {
        offset = Random.Range( -Mathf.PI , Mathf.PI);

        boidLight = GetComponent<Light>();
        if (boidLight == null) { boidLight = this.gameObject.AddComponent<Light>(); }

        boidLight.intensity = intensity;
        boidLight.range = 1.0f;
        boidLight.type = LightType.Point;
        boidLight.enabled = true;

	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localScale = getScale();
        boidLight.intensity = getIntensity();
	}

    public void SetController(BoidController theBoidController)
    {
        boidController = theBoidController;
    }

    private Vector3 getScale()
    {
        float dynamicScale = scale + scale * ( Mathf.Sin(offset + Time.time / 3.0f) / 2.0f);

        return new Vector3(dynamicScale, dynamicScale, dynamicScale);
    }

    private float getIntensity()
    {
        float dynamicIntensity = intensity * (Mathf.Sin(offset + Time.time / 3.0f) - 0.3f);
        if (dynamicIntensity > 0.0f)
        {
            boidLight.enabled = true;
            return dynamicIntensity;
        }
        else
        {
            boidLight.enabled = false;
            return 0.0f;
        }

        
    }

}
