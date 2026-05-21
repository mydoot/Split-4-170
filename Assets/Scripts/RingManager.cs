//using System.Numerics;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

public class RingManager : MonoBehaviour
{
    private Vector3 startpoint;
    private Vector3 currentpoint;

    private Vector3 dragVector;

    private float maxDrag = 10f;

    private LineRenderer line;

    private Rigidbody rb3D;

    public float power = 10f;

    public int trajectoryResolution = 30;
    
    [SerializeField] private Camera cam;

    [SerializeField] private Transform ringTransform; 


    void Start()
    {
        rb3D = this.GetComponent<Rigidbody>();
        cam = Camera.main;
        line = GetComponent<LineRenderer>();

        line.enabled = false;
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
            Vector3 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
            mousePos.z = cam.WorldToScreenPoint(transform.position).z;
            currentpoint = cam.ScreenToWorldPoint(mousePos);

            dragVector = startpoint - currentpoint;
            dragVector = Vector3.ClampMagnitude(dragVector, maxDrag);  

            ShowTrajectory(dragVector * power);
        }
        if (UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame) // modify this to work on clicking the 3d object
        {
            Debug.Log("release");
           
            Vector3 finalForce = dragVector * power;

            //rb3D.useGravity = true;
            rb3D.AddForce(finalForce);

            line.enabled = false;
        } 
    }

    void ShowTrajectory(Vector3 initialForce)
    {
        line.enabled = true;
        line.positionCount = trajectoryResolution;

        Vector3[] points = new Vector3[trajectoryResolution];
        Vector3 velocity = initialForce / rb3D.mass;
        Vector3 startPos = transform.position;

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i * Time.fixedDeltaTime;
            Vector3 pos = startPos + velocity * t + 0.5f * Physics.gravity * t * t;
            points[i] = pos;
        }
        
        line.SetPositions(points);

    }
}
