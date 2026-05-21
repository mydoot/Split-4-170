using UnityEngine;

public class RingManager : MonoBehaviour
{
    private Vector3 startpoint;
    
    [SerializeField] private Transform ringTransform; 


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame) // modify this to work on clicking the 3d object
        {
            Debug.Log("click");
            startpoint = ringTransform.position;
        }
    }
}
