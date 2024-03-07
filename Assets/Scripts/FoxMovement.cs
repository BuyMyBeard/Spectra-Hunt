using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
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
    [SerializeField] float sprintStaminaCost = 15;
    [SerializeField] float timeToConsiderFalling = .5f;
    float currentSpeed = 0;
    new Camera camera;
    Vector2 movementInput;
    Vector3 direction = Vector3.forward;
    float dropSpeed = 0;
    public bool movementFrozen = false;
    public bool movementReduced = false;
    [SerializeField] float turnSpeed = 100;
    Vector2 prevInput = Vector2.zero;
    bool isGrounded = false;
    [SerializeField] float groundCheckDistance = .8f;
    [SerializeField] LayerMask groundLayer;
    bool wasFalling = false;
    float timeSinceWasGrounded = 0;
    Animator animator;

    public Vector3 movementDirection => direction;

    public bool IsSprinting { get; private set; } = false;
    Vector2 previousMovement = Vector2.zero;
    public float Gravity
    {
        get
        {
            dropSpeed += Physics.gravity.y * Time.deltaTime;
            return dropSpeed * Time.deltaTime;
        }
    }
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        camera = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movementFrozen = animator.GetBool("Busy");
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

        wasFalling = isFalling;
        animator.SetBool("IsFalling", isFalling);

        Vector3 relativeMovement;
        if (IsSprinting)
        {
            Vector2 normalizedMovementInput = movementInput.normalized;
            relativeMovement = math.step(.6f, movementInput.magnitude) * Time.deltaTime * new Vector3(normalizedMovementInput.x, 0, normalizedMovementInput.y);
        }
        else
            relativeMovement = math.step(.2f, movementInput.magnitude) * Time.deltaTime * new Vector3(movementInput.x, 0, movementInput.y);

        Vector4 absoluteMovement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * relativeMovement; //handle camera rotation
        Quaternion movementForward;

        if (absoluteMovement.magnitude > 0)
        {
            direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
        }
        // IsSprinting = inputInterface.IsSprinting && inputInterface.Move.magnitude >= runThreshold && stamina.CanRun && !movementReduced && animationEvents.ActionAvailable;

        if (!movementFrozen && direction.magnitude > 0)
        {
            movementForward = Quaternion.LookRotation(direction, Vector3.up);
            characterController.transform.rotation = Quaternion.RotateTowards(transform.rotation, movementForward, turnSpeed * Time.deltaTime);
        }

        if (!movementFrozen)
        {
            animator.SetBool("IsRunning", !movementFrozen && IsSprinting && absoluteMovement.magnitude > 0);
            animator.SetBool("IsWalking", !movementFrozen && !IsSprinting && absoluteMovement.magnitude > 0);
            characterController.Move(absoluteMovement * (IsSprinting ? runningSpeed : walkingSpeed));
        }
        animator.SetFloat("WalkSpeed", movementInput.magnitude);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
            animator.SetTrigger("Scratch");
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
            IsSprinting = true;
        else if (context.canceled)
            IsSprinting = false;
    }

    //void HandleMovement()
    //{
    //    Vector2 movementInput = inputInterface.Move;
    //    if ((prevInput - movementInput).magnitude > 1.05)
    //    {
    //        movementInput = prevInput;
    //        prevInput = Vector2.zero;
    //    }
    //    else
    //        prevInput = movementInput;
    //    float movementMagnitude = movementInput.magnitude;
    //    IsSprinting = inputInterface.IsSprinting && movementMagnitude >= runThreshold && stamina.CanRun && !movementReduced && animationEvents.ActionAvailable;
    //    animator.SetBool("IsSprinting", IsSprinting);
    //    if (movementFrozen)
    //    {
    //        direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
    //        animator.SetBool("IsMoving", false);
    //    }
    //    else if (movementMagnitude >= deadZone) 
    //    {
    //        animator.SetBool("IsMoving", true);
    //        direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);


    //        if (IsSprinting)
    //        {
    //            stamina.Remove(sprintStaminaCost * Time.deltaTime, true);
    //            targetSpeed = targetSpeed > sprintSpeed ? sprintSpeed : targetSpeed + Time.deltaTime * acceleration;
    //            targetSpeed = targetSpeed > sprintSpeed ? sprintSpeed : targetSpeed;
    //        }
    //        else if (movementMagnitude >= runThreshold && !movementReduced)
    //        {
    //            targetSpeed = runningSpeed;
    //            animator.SetFloat("MovementSpeedMultiplier", runningSpeed / walkingSpeed);
    //        }
    //        else
    //        {
    //            targetSpeed = walkingSpeed;
    //            animator.SetFloat("MovementSpeedMultiplier", 1);
    //        }

    //        if (lockOn.IsLocked && !inputInterface.IsSprinting)
    //        {
    //            Vector2 blending = movementInput.normalized;
    //            animator.SetFloat("MovementX", blending.x);
    //            animator.SetFloat("MovementY", blending.y);
    //        }
    //        else
    //        {
    //            animator.SetFloat("MovementX", 0);
    //            animator.SetFloat("MovementY", 1);
    //        }
    //    }
    //    else
    //    {
    //        targetSpeed = 0;
    //        animator.SetBool("IsMoving", false);
    //    }

    //    if (wasSprinting && !IsSprinting) StartCoroutine(Decelerate());
    //    wasSprinting = IsSprinting;
    //    if (!isDecelerating)
    //    {
    //        currentSpeed = targetSpeed;
    //        movementInput = currentSpeed * Time.deltaTime * movementInput.normalized;
    //    }
    //    else if (movementMagnitude < deadZone)
    //    {
    //        movementInput = currentSpeed * Time.deltaTime * previousMovement;
    //    }
    //    else
    //        movementInput = currentSpeed * Time.deltaTime * movementInput.normalized;
    //    movement.x = movementInput.x;
    //    movement.z = movementInput.y;
    //    if (!movementFrozen && movementMagnitude >= deadZone)
    //        previousMovement = movementInput.normalized;
    //}
}