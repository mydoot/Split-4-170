using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Tooltip("This needs to be the prefab from the Assets/Prefab folder.")]
    [SerializeField] RingManager ringPrefab;

    [SerializeField] Transform ringSpawn;

    [SerializeField] TextMeshProUGUI pointText;

    public static GameManager Instance { get; private set; } //Turns GameManager into a singleton

    public static RingManager.OnRingToss onRingToss; //public event for when the ring is thrown;

    private Quaternion originalRingRotation;

    //public variables
    public int Points = 0;

    private void Awake()
    {
        // Checks if the proper GameManager instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); //GameManager goes betweens scenes 
    }

    void Start()
    {
        onRingToss += instantiateNewRing; //GameManager immediately subscribes to the onRingToss event

        originalRingRotation = ringPrefab.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void instantiateNewRing()
    {
        Debug.Log("generating new ring");
        RingManager newRing = Instantiate(ringPrefab, ringSpawn.position, originalRingRotation);
    
    }

    public void gainPoints(int num)
    {
        Points += num;
        pointText.text = $"Points: {Points}";
    }
}
