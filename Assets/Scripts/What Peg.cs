using System.Collections.Generic;
using UnityEngine;

public class BackboardDisplay : MonoBehaviour
{
    [SerializeField] private MeshRenderer displayQuad;
    [SerializeField] private List<Texture2D> pegImages = new List<Texture2D>();
    [SerializeField] private List<Names> expectedPegOrder = new List<Names>();

    private int currentIndex = 0;

    void Start()
    {
        GameManager.onPegLand += OnPegLanded;

        if (displayQuad != null && pegImages.Count > 0)
            displayQuad.material.mainTexture = pegImages[0];
    }

    void OnPegLanded(GameObject peg)
    {
        if (currentIndex >= expectedPegOrder.Count) return;

        if (peg == expectedPegOrder[currentIndex].PegName)
        {
            currentIndex++;

            if (currentIndex < pegImages.Count)
                displayQuad.material.mainTexture = pegImages[currentIndex];
        }
    }

    void OnDestroy()
    {
        GameManager.onPegLand -= OnPegLanded;
    }
}