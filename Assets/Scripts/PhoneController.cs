using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour {

    public States state;

    public enum States
    {
        normal,
        ringing
    }

    // Use this for initialization
    void Start () {

        state = States.normal;
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(state == States.ringing && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
        if (state == States.normal && GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Stop();
        }


    }
}
