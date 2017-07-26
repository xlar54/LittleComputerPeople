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
        left,
        up
    }

    public List<Vector3> waypoints;
    public FacingDirection finalFacing;
    public int floor;
    public int stayMaxTime;

    public Movement()
    {

    }

    public Movement(List<Vector3> waypoints, FacingDirection finalFacing, int floor, int stayMaxTime)
    {
        this.waypoints = waypoints;
        this.finalFacing = finalFacing;
        this.floor = floor;
        this.stayMaxTime = stayMaxTime;
    }



}
