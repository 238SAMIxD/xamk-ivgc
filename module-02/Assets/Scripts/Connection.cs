using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum Direction {
    North,
    East,
    West,
    South
}

public class Connection : MonoBehaviour
{
    public Direction connectionName;
    public string description;
    public Location location;
    public bool isActive;
}
