//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    private Vector3 startpoint;
    private Vector3 currentpoint;

    private float maxDrag = 10f;
    
    [SerializeField] private Camera cam;

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
        if (UnityEngine.InputSystem.Mouse.current.leftButton.isPressed) // modify this to work on clicking the 3d object
        {
            Debug.Log("dragging");
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            currentpoint = cam.ScreenToWorldPoint(mousePos);

            Vector3 dragVector = startpoint - currentpoint;
            dragVector = Vector3.ClampMagnitude(dragVector, maxDrag);


        }
    }
}
