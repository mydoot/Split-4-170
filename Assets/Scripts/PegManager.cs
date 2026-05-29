using System.Collections.Generic;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;



public class PegManager : MonoBehaviour
{

    //[SerializeField] public ScriptableObject pegObject; //Scriptable object is not used but the script PegScriptableObject.cs is used; DO NOT REMOVE IT
    [SerializeField] private GameObject pegParent;

    [SerializeField] private Material successMaterial;
    [SerializeField] private Material defaultMaterial;

    [SerializeField] private GameObject sauronPeg;  

    public List<Names> pegs = new List<Names>();

    [SerializeField] private List<Names> checkPegOrder = new List<Names>();
    [SerializeField] private List<Names> expectedPegOrder = new List<Names>();

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
        if (!hasWon && checkPegOrder.Count == expectedPegOrder.Count)
        {
            hasWon = true;
            GameManager.triggerSignUI?.Invoke("You win!");
            Debug.Log("won!");
        }
    }


    void pegHasRing(GameObject peg)
    {
        Debug.Log("peg has ring");

        Names specificPeg = pegs.Find(p => p.PegName == peg);

         if (specificPeg == null)
        {
            Debug.LogWarning($"Peg not found in list: {peg.name}", this);
            return;
        }

        //sauron ending
        if (specificPeg.PegName == sauronPeg)
        {
            GameManager.triggerSignUI?.Invoke("You lost.");
        }

        int nextIndex = checkPegOrder.Count;
        if (nextIndex >= expectedPegOrder.Count)
        {
            Debug.LogWarning("All pegs have already been landed on. Ignoring additional peg landings.", this);
            return;
        }
        if (specificPeg.PegName != expectedPegOrder[nextIndex].PegName)
        {
            Debug.LogWarning($"Peg landed out of order: {specificPeg.PegName.name}. Expected: {expectedPegOrder[nextIndex].PegName.name}", this);
            return;
        }

        specificPeg.hasRing = true;
        MeshRenderer pegMesh = specificPeg.PegName.GetComponent<MeshRenderer>();
        pegMesh.material = successMaterial;
        checkPegOrder.Add(specificPeg);
        StartCoroutine(ResetPegColor(pegMesh, 2.4f));

    }

    private IEnumerator ResetPegColor(MeshRenderer pegMesh, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (pegMesh != null)
        {
            pegMesh.material = defaultMaterial;
        }
    }

    private void OnDestroy()
    {
        GameManager.onPegLand -= pegHasRing;
    }

}
