using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public int floor = 1;
    private int prevfloor = 1;
    public int speed = 2;

    private Vector3[] floors = new Vector3[3];

	// Use this for initialization
	void Start () {

        floors[0] = new Vector3(0.78f, 7.153f, 12.06f);
        floors[1] = new Vector3(0.78f, 7.893f, 12.06f);
        floors[2] = new Vector3(0.78f, 8.718f, 12.06f);

    }
	
	// Update is called once per frame
	void Update () {
		
        if (prevfloor != floor)
        {
            transform.position = Vector3.MoveTowards(transform.position, floors[floor-1], speed * Time.deltaTime);

            if (transform.position == floors[floor-1])
            {
                prevfloor = floor;
            }
        }

	}
}
