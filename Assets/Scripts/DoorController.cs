using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public enum DoorStates
    {
        open,
        closed
    }

    public DoorStates state = DoorStates.closed;
    private DoorStates prevState = DoorStates.closed;
    public float openAngle = 60;
    public float closedAngle = 0;
    public float speed = 1f;

    private Quaternion targetRotation;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (state == DoorStates.open)
            targetRotation = Quaternion.AngleAxis(openAngle, Vector3.up);
        else
            targetRotation = Quaternion.AngleAxis(closedAngle, Vector3.up);


        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.time);


        if (transform.rotation == targetRotation && prevState != state)
        {
            prevState = state;

            if(state == DoorStates.closed)
                GetComponent<AudioSource>().Play();
        }

    }
}
