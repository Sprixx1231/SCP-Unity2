using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        private bool CanMove { get; } = true;
        private bool isSprinting => canSprint && Input.GetKey(sprintKey);

        //[SerializeField] private bool useFootSteps;
        [Header("Functional Options")] [SerializeField]
        private bool canSprint = true;

        [SerializeField] private bool canHeadBop = true;
        [SerializeField] private bool canInteract = true;
        [SerializeField] private bool useFootsteps = true;
        [SerializeField] private bool useStamina = true;

        [Header("Controls")] [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode interactKey = KeyCode.E;

        [Header("Movement Parameters")] [SerializeField]
        private float walkSpeed = 3.0f;

        [SerializeField] private float sprintSpeed = 3.0f;
        // [SerializeField] private float gravity = 30.0f;

        [Header("Look Parameters")] [SerializeField] [Range(1, 10)]
        private float lookSpeedX = 2.0f;

        [SerializeField] [Range(1, 10)] private float lookSpeedY = 2.0f;
        [SerializeField] [Range(1, 180)] private float upperLookLimit = 80.0f;
        [SerializeField] [Range(1, 180)] private float lowerLookLimit = 80.0f;

        [Header("HeadBop Parameters")] [SerializeField]
        private float walkBobSpeed = 14f;

        [SerializeField] private float walkBobAmount = 0.05f;
        [SerializeField] private float sprintBobSpeed = 18f;
        [SerializeField] private float sprintBobAmount = 0.1f;
        private float _defaultYPos;
        private float _timer;

        [Header("Stamina Parameters")] [SerializeField]
        private float maxStamina = 100.0f;

        [SerializeField] private float staminaUseMultiplier = 5.0f;
        [SerializeField] private float timeBeforeStaminaRegenStarts = 5.0f;
        [SerializeField] private float staminaValueIncrement = 0.001f;
        [SerializeField] private float staminaTimeIncrement = 0.1f;
        private float _currentStamina;
        private Coroutine c_regeneratingStamina;
        public static Action<float> OnStaminaChange;

        [Header("Health Parameters")] [SerializeField]
        private float maxHp = 100.0f;

        [SerializeField] private float timeBeforeRegenStarts = 3.0f;
        [SerializeField] private float healthValueIncrement = 0.001f;
        [SerializeField] private float healthTimeIncrement = 0.1f;
        private float _currentHealth;
        private Coroutine c_regeneratingHealth;
        public static Action<float> OnTakeDamage;
        public static Action<float> OnDamage;
        public static Action<float> OnHeal;

        [Header("Interaction")] [SerializeField]
        private Vector3 interactionRayPoint;

        [SerializeField] private float interactionDistance;
        [SerializeField] private LayerMask interactionLayer;
        private Interactable.Interactable _currentInteractable;

        [Header("FootStep Parameters")] [SerializeField]
        private float baseStepSpeed = 0.5f;

        [SerializeField] private float sprintMultiplier = 0.6f;
        [SerializeField] private AudioSource stepAudioSource;

        [SerializeField] private AudioClip[] walk;

        // [SerializeField] private AudioClip[] run = default;
        [SerializeField] private AudioClip[] metalWalk;

        //[SerializeField] private AudioClip[] metalRun = default;
        private float _footStepTimer;
        private float getOffset => isSprinting ? baseStepSpeed * sprintMultiplier : baseStepSpeed;


        private Camera _playerCamera;
        private CharacterController _characterController;

        private Vector3 _moveDirection;
        private Vector2 _currentInput;

        private float _rotationX;

        public static FPSController Instance;

        private void OnEnable()
        {
            OnTakeDamage += ApplyDamage;
        }

        private void OnDisable()
        {
            OnTakeDamage -= ApplyDamage;
        }

        private void Awake()
        {
            Instance = this;

            _playerCamera = GetComponentInChildren<Camera>();
            _characterController = GetComponent<CharacterController>();
            _defaultYPos = _playerCamera.transform.localPosition.y;
            _currentHealth = maxHp;
            _currentStamina = maxStamina;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (CanMove)
            {
                HandleMovementInput();
                HandleMouseLook();

                if (canHeadBop)
                    HandleHeadBop();

                if (canInteract)
                {
                    HandleInteractionCheck();
                    HandleInteractionInput();
                }

                if (useFootsteps)
                    HandleFootsteps();

                if (useStamina)
                    HandleStamina();


                ApplyFinalMovement();
            }
        }

        private void HandleStamina()
        {
            if (isSprinting && _currentInput != Vector2.zero)
            {
                if (c_regeneratingStamina != null)
                {
                    StopCoroutine(c_regeneratingStamina);
                    c_regeneratingStamina = null;
                }

                _currentStamina -= staminaUseMultiplier * Time.deltaTime;

                if (_currentStamina < 0) _currentStamina = 0;

                OnStaminaChange?.Invoke(_currentStamina);

                if (_currentStamina <= 0) canSprint = false;
            }

            if (!isSprinting && _currentStamina < maxStamina && c_regeneratingStamina == null)
                c_regeneratingStamina = StartCoroutine(RegenStamina());
        }

        private void ApplyDamage(float dmg)
        {
            _currentHealth -= dmg;
            OnDamage?.Invoke(_currentHealth);
            if (_currentHealth <= 0)
                KillPlayer();

            else if (c_regeneratingHealth != null)
                StopCoroutine(c_regeneratingHealth);

            c_regeneratingHealth = StartCoroutine(RegenHealth());
        }

        private void KillPlayer()
        {
            _currentHealth = 0;

            if (c_regeneratingHealth != null) StopCoroutine(c_regeneratingHealth);
            print("dead");
        }

        private void HandleFootsteps()
        {
            if (_currentInput == Vector2.zero) return;

            _footStepTimer -= Time.deltaTime;

            if (_footStepTimer <= 0)
            {
                if (Physics.Raycast(_playerCamera.transform.position, Vector3.down, out var hit, 3))
                    switch (hit.collider.tag)
                    {
                        case "Footsteps/Concrete":
                            stepAudioSource.PlayOneShot(walk[Random.Range(0, walk.Length - 1)]);
                            break;
                        case "Footsteps/Metal":
                            stepAudioSource.PlayOneShot(metalWalk[Random.Range(0, metalWalk.Length - 1)]);
                            break;
                    }

                _footStepTimer = getOffset;
            }
        }

        private void HandleInteractionInput()
        {
            if (Input.GetKeyDown(interactKey) && _currentInteractable != null && Physics.Raycast(
                    _playerCamera.ViewportPointToRay(interactionRayPoint), out var hit, interactionDistance,
                    interactionLayer))
                _currentInteractable.OnInteract();
        }

        private void HandleInteractionCheck()
        {
            if (Physics.Raycast(_playerCamera.ViewportPointToRay(interactionRayPoint), out var hit,
                    interactionDistance))
            {
                if (hit.collider.gameObject.layer == 7 && (_currentInteractable == null ||
                                                           hit.collider.gameObject.GetInstanceID() !=
                                                           _currentInteractable.GetInstanceID()))
                {
                    hit.collider.TryGetComponent(out _currentInteractable);

                    if (_currentInteractable)
                        _currentInteractable.OnFocus();
                }
            }
            else if (_currentInteractable)
            {
                _currentInteractable.OnLoseFocus();
                _currentInteractable = null;
            }
        }


        private void HandleMovementInput()
        {
            _currentInput = new Vector2((isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"),
                (isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

            var moveDirectionY = _moveDirection.y;
            _moveDirection = transform.TransformDirection(Vector3.forward) * _currentInput.x +
                             transform.TransformDirection(Vector3.right) * _currentInput.y;
            _moveDirection.y = moveDirectionY;
        }

        private void HandleMouseLook()
        {
            _rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            _rotationX = Mathf.Clamp(_rotationX, -upperLookLimit, lowerLookLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
        }

        private void HandleHeadBop()
        {
            if (Mathf.Abs(_moveDirection.x) > 0.01f || Mathf.Abs(_moveDirection.z) > 0.1f)
            {
                _timer += Time.deltaTime * (isSprinting ? sprintBobSpeed : walkBobSpeed);

                _playerCamera.transform.localPosition = new Vector3(
                    _playerCamera.transform.localPosition.x,
                    _defaultYPos + Mathf.Sin(_timer) * (isSprinting ? sprintBobAmount : walkBobAmount),
                    _playerCamera.transform.localPosition.z);
            }
        }

        private void ApplyFinalMovement()
        {
            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        private IEnumerator RegenHealth()
        {
            yield return new WaitForSeconds(timeBeforeRegenStarts);
            var timeToWait = new WaitForSeconds(healthTimeIncrement);

            while (_currentHealth < maxHp)
            {
                _currentHealth += healthValueIncrement;

                if (_currentHealth > maxHp) _currentHealth = maxHp;

                OnHeal?.Invoke(_currentHealth);
                yield return timeToWait;
            }

            c_regeneratingHealth = null;
        }

        private IEnumerator RegenStamina()
        {
            yield return new WaitForSeconds(timeBeforeStaminaRegenStarts);
            var timeToWait = new WaitForSeconds(staminaTimeIncrement);

            while (_currentStamina < maxStamina)
            {
                if (_currentStamina > 0) canSprint = true;

                _currentStamina += staminaValueIncrement;

                if (_currentStamina > maxStamina) _currentStamina = maxStamina;

                OnStaminaChange?.Invoke(_currentStamina);

                yield return timeToWait;
            }

            c_regeneratingStamina = null;
        }
    }
}