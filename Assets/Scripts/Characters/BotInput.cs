using UnityEngine;

public class BotInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private Vector3 platformCenter;

    [Header("Sensing Settings")]
    [SerializeField] private float detectionRadius = 4.5f;
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float maxVerticalDistance = 1.2f;

    [Header("Decision Frequency")]
    [SerializeField] private float reevaluateInterval = 0.35f;

    private PlatformTile currentTargetTile;
    private Vector3 moveDirection;
    private float reevaluateTimer;

    private void Awake()
    {
        if (movement == null)
        {
            movement = GetComponent<CharacterMovement>();
        }
    }
    public void SetPlatformCenter(Vector3 position)
    {
        platformCenter = position;
    }
    private void Update()
    {
        reevaluateTimer += Time.deltaTime;

        // Re-evaluate target if timer expires or current target begins breaking
        if (reevaluateTimer >= reevaluateInterval || IsTargetInvalid())
        {
            reevaluateTimer = 0f;
            FindNextTarget();
        }

        // Send input to CharacterMovement
        movement.SetMoveInput(moveDirection);
    }

    private bool IsTargetInvalid()
    {
        if (currentTargetTile == null) 
            return true;
            
        // If the tile started breaking, move away
        if (currentTargetTile.IsBreaking) 
            return true;

        return false;
    }

    private void FindNextTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, platformLayer);

        PlatformTile bestCandidate = null;
        float bestScore = float.MinValue;

        foreach (Collider col in colliders)
        {
            if (!col.TryGetComponent(out PlatformTile tile)) 
                continue;

            // Skip tiles that are already collapsing
            if (tile.IsBreaking) 
                continue;

            // Skip tiles on different height 
            float heightDifference = Mathf.Abs(tile.transform.position.y - transform.position.y);
            if (heightDifference > maxVerticalDistance) 
                continue;

            float distance = Vector3.Distance(transform.position, tile.transform.position);

            // select closer tiles, with slight randomness
            float score = -distance + Random.Range(0f, 1.2f);

            if (score > bestScore)
            {
                bestScore = score;
                bestCandidate = tile;
            }
        }

        currentTargetTile = bestCandidate;

        if (currentTargetTile != null)
        {
            Vector3 direction = currentTargetTile.transform.position - transform.position;
            direction.y = 0f;
            moveDirection = direction.normalized;
        }
        else
        {
            // Move towards center of the main platform when no tile found
            Vector3 directionToCenter = platformCenter - transform.position;
            directionToCenter.y = 0f;

            if (directionToCenter.sqrMagnitude > 0.001f)
            {
                moveDirection = directionToCenter.normalized;
            }
            else
            {
                moveDirection = transform.forward;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Detection Radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}