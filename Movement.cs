using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Movement
{
    public enum FacingDirection
    {
        forward,
        backward,
        right,
        left
    }

    public Movement()
    {

    }

    public Transform transform;
    public FacingDirection finalFacing;
    public List<Transform> waypoints;

}
