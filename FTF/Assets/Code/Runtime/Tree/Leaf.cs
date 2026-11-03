using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Leaf
{
    [Range(0, 1)] public float weatherResistance;
    [Range(0, 1)] public float photosynthesis;
    public Dictionary<Transform, (float lifetime, float resistance, float distance)> leaves = new();
}
