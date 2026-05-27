//using System.Numerics;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

using Vector3 = UnityEngine.Vector3;
using Plane = UnityEngine.Plane;
using Vector2 = UnityEngine.Vector2;
using System.Collections;

public class RingManager : MonoBehaviour
{
    private Vector3 startpoint;
    private Vector3 currentpoint;

    private Vector3 dragVector;

    private float maxDrag = 10f;

    private LineRenderer line;

    private Rigidbody rb3D;

    private Plane dragPlane;

    [Tooltip("Boolean to determine if this ring object was thrown")]
    public bool isThrown = false;

    public delegate void OnRingToss();

    [Tooltip("Power of the ring toss. This value is multiplied by the vector of the drag")]
    public float power = 10f;

    [Tooltip("Amount of seconds a ring will last before it is destroyed")]
    public float lifeTime = 5f;

    [Tooltip("Determines how long the prediction line will be. Bigger value = longer line")]
    public int trajectoryResolution = 30;

    [SerializeField] private Camera cam;

    [SerializeField] private Transform ringTransform;

    [SerializeField] private CapsuleCollider pointCollider;

    [SerializeField] private GameObject clickDetection;

    [Tooltip("Used for the purposes of detecting raycasts on specific objects")]
    public LayerMask raycastOnlyLayer;


    void Start()
    {
        rb3D = GetComponent<Rigidbody>();
        cam = Camera.main;
        line = GetComponent<LineRenderer>();

        line.enabled = false;
    }


    // Had AI assistance for the below
    void Update()
    {
        if (!isThrown)
        {
            if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log("click");

                Vector2 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                Ray clickRay = cam.ScreenPointToRay(mousePos);

                if (Physics.Raycast(clickRay, out RaycastHit hit, 500f, raycastOnlyLayer)) //Check if mouse is clicked on ring
                {
                    //Debug.Log("The raycast successfully hit: " + hit.collider.gameObject.name);
                    if (hit.collider.gameObject == clickDetection)
                    {
                        
                        dragPlane = new Plane(Vector3.up, ringTransform.position);


                        if (dragPlane.Raycast(clickRay, out float distance))
                        {
                            startpoint = ringTransform.position;
                        }

                    }
                }
            }
            if (UnityEngine.InputSystem.Mouse.current.leftButton.isPressed)
            {
                //Debug.Log("dragging");
                Vector2 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

                Ray dragRay = cam.ScreenPointToRay(mousePos);

                if (dragPlane.Raycast(dragRay, out float distance))
                {
                    currentpoint = dragRay.GetPoint(distance);

                    dragVector = startpoint - currentpoint;
                    dragVector = Vector3.ClampMagnitude(dragVector, maxDrag);

                    ShowTrajectory(dragVector * power);
                }
            }
            if (UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Debug.Log("release");
                StartCoroutine(destroyRing());
                isThrown = true;
                Vector3 finalForce = dragVector * power;

                rb3D.useGravity = true;
                rb3D.AddForce(finalForce, ForceMode.Impulse);
                //rb3D.AddForce(-ringTransform.up * (power));

                line.enabled = false;
            }
        }
    }

    void ShowTrajectory(Vector3 initialForce)
    {
        line.enabled = true;
        line.positionCount = trajectoryResolution;

        Vector3[] points = new Vector3[trajectoryResolution];
        Vector3 velocity = initialForce / rb3D.mass;
        Vector3 startPos = ringTransform.position;

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i * Time.fixedDeltaTime;
            Vector3 pos = startPos + velocity * t + 0.5f * Physics.gravity * t * t;
            points[i] = pos;
        }

        line.SetPositions(points);

    }

    IEnumerator destroyRing()
    {
        yield return new WaitForSeconds(lifeTime);
        GameManager.onRingToss?.Invoke(); //calls all methods subscripted to the onRingToss event
        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Peg"))
        {
            Debug.Log("Ring passed into peg");
            GameManager.Instance.gainPoints(1);
        }
    }
}
