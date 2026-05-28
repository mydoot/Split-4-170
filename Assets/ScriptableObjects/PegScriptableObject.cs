using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PegScriptableObject", menuName = "Scriptable Objects/PegScriptableObject")]
public class PegScriptableObject : ScriptableObject
{
    public List<Names> Pegs = new List<Names>();
}

[Serializable]
public class Names
{
    public PegNames PegName;
    public bool hasRing;
}