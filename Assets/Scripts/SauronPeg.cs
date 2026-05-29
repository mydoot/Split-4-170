using UnityEngine;
using System.Collections.Generic;

public class SauronPeg : MonoBehaviour
{
    [Header("Behaviour")]
    [Range(0f, 1f)]
    public float reactionChance = 1f;

    [Header("Movement")]
    public float moveSpeed = 12f;
    public float returnSpeed = 3f;

    [Header("Obstacle Avoidance")]
    public float pegRadius = 0.5f;          
    public float avoidanceStrength = 3f;    // How hard Sauron steers around pegs
    public LayerMask pegLayer;              

    private Vector3 originPosition;
    private Rigidbody ringRb;
    private bool isReacting = false;
    private float targetX;

    void Start()
    {
        originPosition = transform.position;
        targetX = originPosition.x;
        GameManager.onRingToss += OnNewRing;
    }

    void OnDestroy()
    {
        GameManager.onRingToss -= OnNewRing;
    }

    void OnNewRing()
    {
        isReacting = false;
        ringRb = null;
        targetX = originPosition.x;
    }

    public void NotifyRingThrown(Rigidbody thrownRingRb)
    {
        if (isReacting) return;

        if (Random.value <= reactionChance)
        {
            ringRb = thrownRingRb;
            isReacting = true;
            StartCoroutine(PredictAfterPhysicsUpdate());
        }
    }

    private System.Collections.IEnumerator PredictAfterPhysicsUpdate()
    {
        yield return new WaitForFixedUpdate(); // wait one physics tick for velocity to be an actual value
        if (ringRb != null)
            targetX = PredictLandingX();
    }

    void Update()
    {
        float speed = isReacting ? moveSpeed : returnSpeed;
        float desiredX = isReacting ? targetX : originPosition.x;

        // Avoidance: check for pegs nearby and move away
        desiredX = ApplyAvoidance(desiredX);

        float newX = Mathf.MoveTowards(transform.position.x, desiredX, speed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    float ApplyAvoidance(float desiredX)
    {
        // Find all nearby pegs
        Collider[] nearby = Physics.OverlapSphere(transform.position, pegRadius * 4f, pegLayer);

        foreach (Collider col in nearby)
        {
            if (col.gameObject == gameObject) continue;

            float xDiff = transform.position.x - col.transform.position.x;
            float xDist = Mathf.Abs(xDiff);

            // Only pegs within touching range on X
            if (xDist < pegRadius * 2f)
            {
                float push = (pegRadius * 2f - xDist) * avoidanceStrength;
                // Push away from the peg's X position
                desiredX += Mathf.Sign(xDiff) * push;
            }
        }

        return desiredX;
    }

    float PredictLandingX()
    {
        Vector3 pos = ringRb.position;
        Vector3 vel = ringRb.linearVelocity;

        float zDist = transform.position.z - pos.z;
        if (Mathf.Abs(vel.z) < 0.01f) return pos.x;

        float timeToReach = zDist / vel.z;
        return pos.x + vel.x * timeToReach;
    }
}