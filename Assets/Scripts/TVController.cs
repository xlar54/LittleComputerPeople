using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : MonoBehaviour {

    public float timer = 0.0f;
    public States state;

    public enum States
    {
        on,
        off
    }

    // Use this for initialization
    void Start () {

        state = States.off;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (state == States.on)
        {
            timer += Time.deltaTime;
            if (timer >= 0.25f)
            {
                Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
                GetComponent<Renderer>().material.color = newColor;
                timer = 0;
            }
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.black;
        }



        
		
	}
}
