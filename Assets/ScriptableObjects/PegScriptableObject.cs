using System;
using System.Collections.Generic;
using UnityEngine;

//DO NOT DELETE THIS SCRIPT; the class Names is used

[CreateAssetMenu(fileName = "PegScriptableObject", menuName = "Scriptable Objects/PegScriptableObject")]
public class PegScriptableObject : ScriptableObject
{
    public List<Names> Pegs = new List<Names>();
}


[Serializable]
public class Names
{
    public GameObject PegName;
    public bool hasRing;

    public Names(GameObject peg, bool boolean)
    {
        PegName = peg;
        hasRing = boolean;
    }
}