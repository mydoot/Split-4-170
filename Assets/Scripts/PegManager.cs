using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;



public class PegManager : MonoBehaviour
{

    //[SerializeField] public ScriptableObject pegObject; //Scriptable object is not used but the script PegScriptableObject.cs is used; DO NOT REMOVE IT
    [SerializeField] private GameObject pegParent;

    [SerializeField] private Material successMaterial;

    public List<Names> pegs = new List<Names>();

    [SerializeField] private List<Names> checkPegOrder = new List<Names>();

    public bool hasWon = false;

    public delegate void OnPegLand(GameObject peg);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.onPegLand += pegHasRing;

        // Grabs each peg child of the "Pegs" game obejct.
        foreach (Transform Child in pegParent.GetComponentInChildren<Transform>())
        {
            Names peg = new Names(Child.gameObject, false);
            pegs.Add(peg);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasWon)
        {
            if (checkPegOrder.SequenceEqual(pegs))
            {
                if (pegs.All(pegs => pegs.hasRing))
                {
                    hasWon = true;
                    GameManager.triggerSignUI?.Invoke("You win!");
                    Debug.Log("won!");
                }
            }
        }
    }


    void pegHasRing(GameObject peg)
    {
        Debug.Log("peg has ring");

        Names specificPeg = pegs.Find(p => p.PegName == peg);

        if (!specificPeg.hasRing)
        {
            specificPeg.hasRing = true;
            MeshRenderer pegMesh = specificPeg.PegName.GetComponent<MeshRenderer>();
            pegMesh.material = successMaterial;

            checkPegOrder.Add(specificPeg);
        }

    }

}
