using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using QFSW.QC;

public class FirstPersonController : NetworkBehaviour
{
    public bool CanMove { get; private set; } = true;
    private bool ConsoleOpened { get { return quantumConsole.IsFocused; } } // get console value
    private bool isSprinting => canSprint && Input.GetKey(sprintKey);
    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded && !isCrouching;
    private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;
    private bool toggleInventory => Input.GetKeyDown(InventoryUIKey);

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadBob = true;
    [SerializeField] private bool willSlideOnSlopes = true;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool canPickUpObjects = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool useStamina = true;
    [SerializeField] private bool useFlashlight = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode InteractKey = KeyCode.E;
    [SerializeField] private KeyCode InventoryUIKey = KeyCode.Tab;
    //[SerializeField] private KeyCode PickUpKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode PickUpKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode DropKey = KeyCode.G;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float slopeSpeed = 6f;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Health Parameters")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float timeBeforeRegenStarts = 3;
    [SerializeField] private float healthValueIncrement = 1;
    [SerializeField] private float healthTimeIncrement = 0.1f;
    private float currentHealth;
    private Coroutine regeneratingHealth;
    public static Action<float> OnTakeDamage;
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;

    [Header("Stamina Parameters")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaUseMultiplier = 10;
    [SerializeField] private float timeBeforeStaminaRegenStarts = 5;
    [SerializeField] private float staminaValueIncrement = 2;
    [SerializeField] private float staminaTimeIncrement = 0.1f;
    private float currentStamina;
    private Coroutine regeneratingStamina;
    public static Action<float> OnStaminaChange;

    public float GetmaxStamina { get { return maxStamina; } }
    public float GetCurrentStamina { get { return currentStamina; } }

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.02f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.05f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.01f;
    private float defaultYPos = 0;
    private float bobTimer;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.3f;
    [SerializeField] private float zoomFOV = 30f;
    private float defaultFOV;
    private Coroutine zoomRoutine;

    [Header("Footsteps parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] defaultStep = default;

    //[SerializeField] private AudioClip[] woodClips = default;
    //[SerializeField] private AudioClip[] metalClips = default;
    //[SerializeField] private AudioClip[] grassClips = default;
    private float footStepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = new Vector3(0.5f, 0.5f, 0);
    [SerializeField] private float interactionDistance = 2;
    private LayerMask interactionLayer = default;
    private LayerMask interactionIgnoreLayer = 0 | 1 << 7;
    private InteractableObject currentInteractable;

    [Header("PickUp")]
    private Transform pickUpPoint;
    public Transform PickUpPoint { get { return pickUpPoint; } }
    private LayerMask pickUpLayer = default;
    private LayerMask pickUpIgnoreLayer = 0 | 1 << 6;
    private PickUpObject currentPickUpObject;
    private bool objectInHand = false;

    [Header("Flashlight")]
    [SerializeField] private GameObject Flashlight;
    private bool flashOn = false;
    private float maxIntensity = 100;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;

    [Header("SoundRange")]
    [SerializeField] private float soundRange;

    [Header("HUD")]
    [SerializeField] private GameObject HUD;

    /*SLIDING PARAMETERS*/
    private Vector3 hitPointNormal;
    private bool IsSliding
    {
        get
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else return false;
        }
    }

    public Camera playerCamera;
    public CharacterController characterController;

    [Header("Character")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject playerCharacter;

    // LOCAL Y of playerCharacter
    // -0.4 y unCrouched
    // 0.3 y crouched
    // 0.55 y crouchedWalking
    private Vector3 unCrouched = new(0, -0.4f, 0);
    private Vector3 crouched = new(0, 0.3f, 0);
    private Vector3 crouchedWalking = new(0, 0.55f, 0);

    private Inventory inventory;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    public static FirstPersonController instance;

    [Header("Console")]
    private QuantumConsole quantumConsole;
    private bool isFlying;
    public bool IsFlying { get { return isFlying; } set { isFlying = value; } }
    public float Gravity { get { return gravity; } set { gravity = value; } }
    public bool UseStamina { get { return useStamina; } set {  useStamina = value; } }
    public float WalkSpeed { get { return walkSpeed; } set {  walkSpeed = value; } }
    public float SprintSpeed {  get { return sprintSpeed; } set {  sprintSpeed = value; } }
    public float CrouchSpeed { get { return crouchSpeed; } set {  crouchSpeed = value; } }


    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }

    public override void OnNetworkSpawn()
    {
        //if(!IsOwner) return;

        if (IsOwner)
        {
            instance = this;

            transform.position = new Vector3(150, 100, 150);
            //Debug.Log("Position set to: " + transform.position);
            Physics.SyncTransforms();

            listener.enabled = true;
            vc.Priority = 10;

            playerCamera = GetComponentInChildren<Camera>();
            defaultYPos = playerCamera.transform.localPosition.y;
            defaultFOV = playerCamera.fieldOfView;

            characterController = GetComponent<CharacterController>();

            currentHealth = maxHealth;
            currentStamina = maxStamina;

            pickUpPoint = GetComponentInChildren<Camera>().transform.Find("PickUpPoint");

            Flashlight.GetComponent<Light>().intensity = maxIntensity;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // hide local players playercharacter, will still show from other players view
            SkinnedMeshRenderer[] characterModel = playerCharacter.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var child in characterModel)
            {
                child.enabled = false;
            }

            quantumConsole = GameObject.Find("Quantum Console").GetComponent<QuantumConsole>();
            
        }
        else
        {
            Flashlight.GetComponent<Light>().intensity = 0;
            vc.Priority = 0;
            HUD.SetActive(false);
        }
    }

    private void Start()
    {
        interactionLayer = LayerMask.NameToLayer("Interactable");
        pickUpLayer = LayerMask.NameToLayer("PickUp");
    }

    void Update()
    {
        //if(!IsOwner) return;

        if (IsOwner)
        {
            CanMove = !ConsoleOpened; // stäng av movement om konsollen är öppen

            if (CanMove)
            {
                HandleMovementInput();
                HandleMouseLook();

                if (IsFlying)
                    HandleFlying();

                if (canJump)
                    HandleJump();

                if (canCrouch)
                    HandleCrouch();

                if (canUseHeadBob)
                    HandleHeadBob();

                if (canZoom)
                    HandleZoom();

                if (toggleInventory) 
                    HandleInventoryToggle();

                if (useFootsteps)
                    HandleFootsteps();

                if (canInteract)
                {
                    HandleInteractionCheck();
                    HandleInteractionInput();
                }

                if (canPickUpObjects)
                {
                    HandlePickUpsCheck();
                    HandlePickUpsInput();
                }

                if (useFlashlight)
                    HandleFlashLight();

                if (useStamina)
                {
                    HandleStamina();
                }

                OffsetLocalPlayerModel();

                ApplyFinalMovements();
            }
        }
    }

    // offset player character based on state (Model is fucked)
    private void OffsetLocalPlayerModel()
    {
        if (isCrouching && (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f))
        {
            playerCharacter.transform.localPosition = crouchedWalking;
        }
        else if (isCrouching)
        {
            playerCharacter.transform.localPosition = crouched;
        }
        else
        {
            playerCharacter.transform.localPosition = unCrouched;
        }
    }

    private void HandleFlying()
    {
        if (Input.GetKey(crouchKey))
        {
            moveDirection.y = -walkSpeed;
        }
        else if (Input.GetKey(jumpKey))
        {
            moveDirection.y = walkSpeed;
        }
        else
        {
            moveDirection.y = 0;
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), 
            (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        animator.SetBool("isRunning", isSprinting); // check if is sprinting and sending message to animator

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) 
            + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }
     
    private void HandleJump()
    {
        // isJumping animation is played once, could be better to use trigger but im lazy
        if (shouldJump)
        {
            animator.SetBool("isJumping", true);
            moveDirection.y = jumpForce;
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void HandleCrouch()
    {
        if (shouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }

        animator.SetBool("isCrouched", isCrouching);
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        vc.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleInventoryToggle()
    {
        InventoryActions.OnInventoryToggle();
    }

    private void ApplyDamage(float dmg)
    {
        currentHealth -= dmg;
        OnDamage?.Invoke(currentHealth);

        if (currentHealth <= 0)
            KillPlayer();
        else if (regeneratingHealth != null)
            StopCoroutine(regeneratingHealth);

        regeneratingHealth = StartCoroutine(RegenerateHealth());

    }

    private void KillPlayer()
    {
        currentHealth = 0;

        if (regeneratingHealth != null)
            StopCoroutine(regeneratingHealth);

        //Handle death
        print("DEAD");

    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }

        if (Input.GetKeyUp(zoomKey))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandleHeadBob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            bobTimer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            vc.transform.localPosition = new Vector3(
                vc.transform.localPosition.x,
                defaultYPos + Mathf.Sin(bobTimer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount),
                vc.transform.localPosition.z);
        }
    }

    private void HandleStamina()
    {
        if (isSprinting && currentInput != Vector2.zero)
        {

            if(regeneratingStamina != null)
            {
                StopCoroutine (regeneratingStamina);
                regeneratingStamina = null;
            }

            currentStamina -= staminaUseMultiplier * Time.deltaTime; 

            if (currentStamina < 0)
                currentStamina = 0;

            OnStaminaChange?.Invoke(currentStamina);

            if (currentStamina <= 0)
                canSprint = false;
        }

        if (!isSprinting && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
        }
    }

    private void HandleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint),
            out RaycastHit hit, interactionDistance, interactionIgnoreLayer))
        {
            if (hit.collider.gameObject.layer == interactionLayer && 
                (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                if (currentInteractable != null)
                {
                    currentInteractable.OnLoseFocus();
                }
                hit.collider.TryGetComponent(out currentInteractable);

                if(currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(InteractKey) && currentInteractable != null 
            && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), 
            out RaycastHit hit, interactionDistance, interactionIgnoreLayer)) 
        {
            currentInteractable.OnInteract();
        }
    }

    private void HandlePickUpsCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint),
            out RaycastHit hit, interactionDistance, pickUpIgnoreLayer) && !objectInHand)
        {
            if (hit.collider.gameObject.layer == pickUpLayer &&
                (currentPickUpObject == null || hit.collider.gameObject.GetInstanceID() != currentPickUpObject.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentPickUpObject);

                if (currentPickUpObject)
                    currentPickUpObject.OnFocus();

            }
        }
        else if (currentPickUpObject && !objectInHand)
        {
            currentPickUpObject.OnLoseFocus();
            currentPickUpObject = null;
        }
    }


    // changes owner of a given networkobjectid to local players id, sends command to server and forces server to change it
    [ServerRpc(RequireOwnership = false)]
    private void RequestOwnershipServerRpc(ulong objectToChange,ServerRpcParams serverRpcParams = default)
    {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectToChange].GetComponent<NetworkObject>().ChangeOwnership(serverRpcParams.Receive.SenderClientId);
    }


    private void HandlePickUpsInput()
    {
        if (Input.GetKeyDown(PickUpKey) && currentPickUpObject != null
            && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint),
            out RaycastHit hit, interactionDistance, pickUpIgnoreLayer) && !objectInHand)
        {
            if (!hit.collider.gameObject.GetComponent<PickUpObject>().IsObjectAvailable) return; // check if object is not picked up by another player

            RequestOwnershipServerRpc(hit.collider.gameObject.GetComponent<NetworkObject>().NetworkObjectId); // makes new owner manage physics of object when picked up

            currentPickUpObject.OnInteract();
            currentPickUpObject.OnLoseFocus();
            objectInHand = true;
        }
        else if (Input.GetKeyDown(PickUpKey) && objectInHand)
        {
            currentPickUpObject.Throw();
            objectInHand = false;
        }
        else if (Input.GetKeyDown(DropKey) && objectInHand)
        {
            currentPickUpObject.Drop();
            objectInHand = false;
        }
    }

    private void HandleFlashLight()
    {
        if (Input.GetKeyDown(flashlightKey))
        {
            flashOn = !flashOn;
        }

        Flashlight.GetComponent<Light>().enabled = flashOn;

        if (flashOn)
        {
            Flashlight.GetComponent<Light>().intensity -= Time.deltaTime * 3;
        }
        else
        {
            Flashlight.GetComponent<Light>().intensity += Time.deltaTime * 2;
        }

        if (Flashlight.GetComponent<Light>().intensity >= maxIntensity)
        {
            Flashlight.GetComponent<Light>().intensity = maxIntensity;
        }

        if (Flashlight.GetComponent<Light>().intensity <= 0)
        {
            flashOn = false;
        }
        
    }

    private void HandleFootsteps()
    {
        float SoundRange;
        float rangefactor;
        
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footStepTimer -= Time.deltaTime;

        if (footStepTimer <= 0)
        {
            //kollar fr�n under spelaren, inte under kameran
            if(Physics.Raycast(characterController.transform.position, Vector3.down, out RaycastHit hit, 3))
            {

                footstepAudioSource.PlayOneShot(defaultStep[UnityEngine.Random.Range(0, defaultStep.Length)]);

                //switch (hit.collider.tag)
                //{
                //    //Add different tags for different materials, these are examples:
                //    case "Footsteps/GRASS":
                //        footstepAudioSource.PlayOneShot(grassClips[UnityEngine.Random.Range(0, grassClips.Length - 1)]);
                //        break;
                //    case "Footsteps/WOOD":
                //        footstepAudioSource.PlayOneShot(woodClips[UnityEngine.Random.Range(0, woodClips.Length - 1)]);
                //        break;
                //    case "Footsteps/METAL":
                //        footstepAudioSource.PlayOneShot(metalClips[UnityEngine.Random.Range(0, metalClips.Length - 1)]);
                //        break;
                //    default:
                //        footstepAudioSource.PlayOneShot(defaultStep[UnityEngine.Random.Range(0, defaultStep.Length - 1)]);
                //        break;
                //}
            }

            footStepTimer = GetCurrentOffset;

            if (isCrouching)
            {
                footstepAudioSource.volume = 0.25f;
                rangefactor = 0.5f;
                
            }
            else if (isSprinting)
            {
                footstepAudioSource.volume = 1f;
                rangefactor = 2f;
            }
            else
            {
                footstepAudioSource.volume = 0.5f;
                rangefactor = 1f;
            }

            SoundRange = soundRange * rangefactor;

            Sound sound = ScriptableObject.CreateInstance<Sound>();
            sound.Initialize(transform.position, SoundRange); // Assuming hit.point is where the sound originates

            // Pass the sound to the Sounds manager
            Sounds.MakeSound(sound);
            Debug.Log($"Sound: with pos {sound.pos} and range {sound.range} created!");
        }
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // hella lit way to send movement state to animator
        if(Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (willSlideOnSlopes && IsSliding)
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(timeBeforeRegenStarts);

        WaitForSeconds timeToWait = new WaitForSeconds(healthTimeIncrement);

        while (currentHealth < maxHealth)
        {
            currentHealth += healthValueIncrement;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            OnHeal?.Invoke(currentHealth);
            yield return timeToWait;
        }

        regeneratingHealth = null;

    }

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(timeBeforeStaminaRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);

        while (currentStamina < maxStamina)
        {
            if (currentStamina > 0)
                canSprint = true;

            currentStamina += staminaValueIncrement;

            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            OnStaminaChange?.Invoke(currentStamina);

            yield return timeToWait;
        }

        regeneratingStamina = null;
    }

    private IEnumerator CrouchStand()
    {
        animator.SetBool("isCrouched", isCrouching); // just setting isCrouched state to isCrouching, could also be a trigger but im still lazy

        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed/timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = vc.m_Lens.FieldOfView;
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            vc.m_Lens.FieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed/timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        vc.m_Lens.FieldOfView = targetFOV;
        zoomRoutine = null;
    }
}
