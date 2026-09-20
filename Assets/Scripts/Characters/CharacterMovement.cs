using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -25f;

    [Header("Auto Jump")]
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float platformCheckDistance = 0.9f;
    [SerializeField] private Vector3 platformCheckHalfExtents = new Vector3(0.35f, 0.15f, 0.35f);
    [SerializeField] private float platformCheckHeight = 0.15f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private CharacterController controller;

    private Vector3 moveInput;
    private float verticalVelocity;
    private bool isJumping;
    private bool wasGrounded;
    private bool isPlayer;
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int InAirHash = Animator.StringToHash("InAir");
    private static readonly int LandHash = Animator.StringToHash("Land");

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        isPlayer = gameObject.CompareTag("Player")? true: false;
    }

    private void Start()
    {
        wasGrounded = controller.isGrounded;
    }

    private void Update()
    {
        bool groundedBeforeMovement = controller.isGrounded;

        HandleAutoJump(groundedBeforeMovement);
        HandleGravity();

        MoveCharacter();

        bool groundedAfterMovement = controller.isGrounded;

        HandleLanding(groundedBeforeMovement, groundedAfterMovement);
        UpdateAnimation(groundedAfterMovement);
    }

    // Called by PlayerInput or Bot AI.
    public void SetMoveInput(Vector3 input)
    {
        input.y = 0f;
        moveInput = Vector3.ClampMagnitude(input, 1f);
    }

    private void MoveCharacter()
    {
        Vector3 horizontalVelocity = moveInput * moveSpeed;

        Vector3 velocity = horizontalVelocity;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        RotateTowardsMovement();
    }

    private void RotateTowardsMovement()
    {
        if (moveInput.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveInput);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void HandleAutoJump(bool grounded)
    {
        if (!grounded || isJumping)
            return;

        if (moveInput.sqrMagnitude < 0.01f)
            return;

        if (!HasPlatformAhead())
        {
            Jump();
        }
    }

    private bool HasPlatformAhead()
    {
        Vector3 checkPosition =transform.position +moveInput.normalized * platformCheckDistance +Vector3.up * platformCheckHeight;
        return Physics.CheckBox(checkPosition,platformCheckHalfExtents,Quaternion.identity,platformLayer,QueryTriggerInteraction.Ignore);
    }

    private void Jump()
    {
        isJumping = true;

        if(AudioManager.Instance!= null && isPlayer)
            AudioManager.Instance.PlayJumpSfx();

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        animator.SetTrigger(JumpHash);
        animator.SetBool(InAirHash, true);
    }

    private void HandleLanding(bool groundedBefore, bool groundedAfter)
    {
        // Trigger when character touches ground after being in air
        if (!groundedBefore && groundedAfter)
        {
            isJumping = false;
            animator.SetBool(InAirHash, false);
            animator.SetTrigger(LandHash);
        }

        wasGrounded = groundedAfter;
    }

    private void UpdateAnimation(bool grounded)
    {
        float speed = moveInput.magnitude;
        animator.SetFloat(SpeedHash, speed);

        // sync InAir parameter with actual ground state
        animator.SetBool(InAirHash, !grounded);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 direction = transform.forward;
        Vector3 checkPosition = transform.position + direction * platformCheckDistance + Vector3.up * platformCheckHeight;
        Gizmos.DrawWireCube(checkPosition, platformCheckHalfExtents * 2f);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.TryGetComponent(out PlatformTile tile))
        {
            if (!tile.IsBreaking)
            {
                tile.CharacterStepped(isPlayer);
            }
        }
    }
}