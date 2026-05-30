using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private float tiltAmount = 5f;
    [SerializeField] private float tiltSpeed = 2f;

    private float targetXRotation;
    private float originalXRotation;
    private bool isTilted = false;

    void Start()
    {
        originalXRotation = transform.eulerAngles.x;
        targetXRotation = originalXRotation;
    }

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            isTilted = !isTilted;
            targetXRotation = isTilted
                ? originalXRotation - tiltAmount
                : originalXRotation;
        }

        float currentX = transform.eulerAngles.x;
        float newX = Mathf.LerpAngle(currentX, targetXRotation, Time.deltaTime * tiltSpeed);
        transform.eulerAngles = new Vector3(newX, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}