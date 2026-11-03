using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TreePollen
{
    public Flower flower;
    public FlowerModifiers flowerModifiers;
    public Dictionary<Transform, (Pollen state, float lifetime, float resistance)> pollens = new();
}
