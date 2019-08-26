using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {

    public float LifeTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //can set the time until the object despawns.
        LifeTime -= Time.deltaTime;

        if(LifeTime <= 0)
        {
            //destroys object once timer ends.
            Destroy(gameObject);
        }
	}
}
