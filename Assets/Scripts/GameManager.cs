using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum PegNames
{
    name1, name2, name3, name4, name5, name6, name7, name8, name9, name10
}

public class GameManager : MonoBehaviour
{
    [Tooltip("This needs to be the prefab from the Assets/Prefab folder.")]
    [SerializeField] RingManager ringPrefab;

    [SerializeField] Transform ringSpawn;

    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] TextMeshProUGUI ringText;

    public static GameManager Instance { get; private set; } //Turns GameManager into a singleton

    public static RingManager.OnRingToss onRingToss; //public event for when the ring is thrown;

    private Quaternion originalRingRotation;

    //public variables
    public int Points = 0;

    public int ringCount = 10;

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

        ringText.text = $"Rings Remaining: {ringCount}";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void instantiateNewRing()
    {
        if (ringCount > 0)
        {
            Debug.Log("generating new ring");
            RingManager newRing = Instantiate(ringPrefab, ringSpawn.position, originalRingRotation);
            ringText.text = $"Rings Remaining: {--ringCount}";
        }
        else
            Debug.Log("no more rings!");
    }

    public void gainPoints(int num)
    {
        Points += num;
        pointText.text = $"Points: {Points}";
    }
}
