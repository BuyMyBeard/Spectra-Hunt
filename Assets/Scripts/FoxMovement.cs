using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(FoxSound))]
public class FoxMovement : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float walkingSpeed = 1;
    [SerializeField] float runningSpeed = 2;
    [Range(0, 1)]
    [SerializeField] float runThreshold = .7f;
    [Range(0, 1)]
    [SerializeField] float deadZone = .1f;
    [Range(0, 50)]
    [SerializeField] float acceleration = 5;
    [Range(0,50)]
    [SerializeField] float deceleration = 15;
    [Tooltip("Stamina/s")]
    [SerializeField] float staminaRunCost = 15;
    [SerializeField] float timeToConsiderFalling = .5f;
    [SerializeField] Transform camFollowTarget;
    Vector2 movementInput;
    Vector3 direction = Vector3.forward;
    float dropSpeed = 0;
    public bool movementFrozen = false;
    [SerializeField] float turnSpeed = 100;
    bool isGrounded = false;
    [SerializeField] float groundCheckDistance = .8f;
    [SerializeField] LayerMask groundLayer;
    float timeSinceWasGrounded = 0;
    Animator animator;
    Stamina stamina;
    FoxSound foxSound;
    float attackTimer;
    [SerializeField] float attackCooldown = 2;
    [SerializeField] float baseStaminaAttackCost = 4;
    [SerializeField] float runAttackRootMultiplier = 5;
    [SerializeField] float walkAttackRootMultiplier = 3;
    [SerializeField] float idleAttackRootMultiplier = 2;

    public Vector3 movementDirection => direction;

    private bool RunInput { get; set; } = false;
    public bool IsRunning { get; private set; } = false;
    public bool isWalking { get; private set; }

    public float Gravity
    {
        get
        {
            dropSpeed += Physics.gravity.y * Time.deltaTime;
            return dropSpeed * Time.deltaTime;
        }
    }

    public bool SneakInput { get; private set; } = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        stamina = GetComponent<Stamina>();
        foxSound = GetComponent<FoxSound>();
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;
        movementFrozen = animator.GetBool("IsBusy");
        characterController.Move(new Vector3(0, Gravity, 0));
        isGrounded = characterController.isGrounded;
        bool isFalling = !isGrounded;

        if (characterController.isGrounded)
        {
            dropSpeed = -1f;
            timeSinceWasGrounded = 0;
        }
        else
        {
            timeSinceWasGrounded += Time.deltaTime;
            if (timeSinceWasGrounded > timeToConsiderFalling)
                isFalling = !(dropSpeed < 0 && Physics.Raycast(characterController.transform.position, Vector3.down, groundCheckDistance, groundLayer));
            else isFalling = false;
        }

        animator.SetBool("IsFalling", isFalling);

        if (movementFrozen) return;

        Vector3 relativeMovement;
        if (RunInput && !SneakInput)
        {
            Vector2 normalizedMovementInput = movementInput.normalized;
            relativeMovement = math.step(.6f, movementInput.magnitude) * Time.deltaTime * new Vector3(normalizedMovementInput.x, 0, normalizedMovementInput.y);
        }
        else
            relativeMovement = math.step(.2f, movementInput.magnitude) * Time.deltaTime * new Vector3(movementInput.x, 0, movementInput.y);

        Vector4 absoluteMovement = Quaternion.Euler(0, camFollowTarget.transform.eulerAngles.y, 0) * relativeMovement; //handle camera rotation
        Quaternion movementForward;

        if (absoluteMovement.magnitude > 0)
        {
            direction = Quaternion.Euler(0, camFollowTarget.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
        }

        if (direction.magnitude > 0)
        {
            movementForward = Quaternion.LookRotation(direction, Vector3.up);
            characterController.transform.rotation = Quaternion.RotateTowards(transform.rotation, movementForward, turnSpeed * Time.deltaTime);
        }

        IsRunning = stamina.CanRun && RunInput && !SneakInput && absoluteMovement.magnitude > 0;
        isWalking = absoluteMovement.magnitude > 0;
        animator.SetBool("IsRunning", IsRunning);
        animator.SetBool("IsWalking", !movementFrozen && (!IsRunning || SneakInput) && absoluteMovement.magnitude > 0);
        characterController.Move((IsRunning ? runningSpeed : walkingSpeed) * (SneakInput ? .5f : 1f) * absoluteMovement);
        animator.SetFloat("WalkSpeed", movementInput.magnitude * (SneakInput ? .5f : 1f));

        if (IsRunning)
        {
            stamina.Remove(Time.deltaTime * staminaRunCost);
            foxSound.NoiseLevel = 1;
        }
        else if (SneakInput || movementInput.magnitude < .5f) foxSound.NoiseLevel = 0;
        else foxSound.NoiseLevel = .5f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        float multiplier;
        if (IsRunning) multiplier = runAttackRootMultiplier;
        else if (isWalking) multiplier = walkAttackRootMultiplier;
        else multiplier = idleAttackRootMultiplier;

        if (context.started && !animator.GetBool("IsBusy") && stamina.Value > baseStaminaAttackCost * multiplier && attackTimer <= 0)
        {
            attackTimer = attackCooldown;
            stamina.Remove(baseStaminaAttackCost * multiplier);
            animator.SetTrigger("Attack");
            stamina.StopRegen();
            animator.SetBool("IsMirrored", UnityEngine.Random.Range(0, 2) == 1);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
            RunInput = true;
        else if (context.canceled)
            RunInput = false;
    }

    public void OnSneak(InputAction.CallbackContext context)
    {
        if (context.performed)
            SneakInput = true;
        else if (context.canceled)
            SneakInput = false;
    }
    private void OnAnimatorMove()
    {
        float multiplier;
        if (IsRunning) multiplier = runAttackRootMultiplier;
        else if (isWalking) multiplier = walkAttackRootMultiplier;
        else multiplier = idleAttackRootMultiplier;
        characterController.Move(animator.deltaPosition * multiplier);
    }
}