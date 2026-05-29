using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

    [SerializeField] Transform endObjectTransform;
    private Quaternion originalRingRotation; 
    //singleton creation
    public static GameManager Instance { get; private set; } //Turns GameManager into a singleton

    // Public events (delegates)
    public delegate void TriggerSignUI(string text);
    public static TriggerSignUI triggerSignUI; //public event for when the ring is thrown

    public static RingManager.OnRingToss onRingToss; //public event for when the ring is thrown

    public static PegManager.OnPegLand onPegLand; //public event for when a ring lands in a peg

    

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
        //DontDestroyOnLoad(gameObject); //GameManager goes betweens scenes
        onRingToss = null;
        triggerSignUI = null;
        onPegLand = null;
    }

    void Start()
    {
        onRingToss += instantiateNewRing; //GameManager immediately subscribes to the onRingToss event
        triggerSignUI += triggerEndScreen;
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
        {
            Debug.Log("no more rings!");
            triggerSignUI?.Invoke("Good try!");
        }
    }

    public void gainPoints(int num)
    {
        Points += num;
        pointText.text = $"Points: {Points}";
    }

    public void triggerEndScreen(string text)
    {
        endObjectTransform.gameObject.SetActive(true);
        endObjectTransform.DOMove(endObjectTransform.up * 10, 1f).From(true).SetEase(Ease.OutBounce);
    }

    public void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Points = 0;
        ringCount = 10;
    }
}
