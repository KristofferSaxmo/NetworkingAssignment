using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static PlayerInput;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour, IPlayerActions
{
    private PlayerInput _playerInput;
    private Vector2 _moveInput;
    private Vector2 _cursorLocation;

    private Rigidbody2D _rb;

    private Transform turretPivotTransform;

    private NetworkVariable<bool> _isMoving = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private SpriteRenderer _spriteRenderer;
    
    public UnityAction<bool> onFireEvent;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float shipRotationSpeed = 100f;
    [SerializeField] private float turretRotationSpeed = 4f;
    
    [FormerlySerializedAs("movingSprites")]
    [Header("Sprites Settings")]
    [SerializeField] private Sprite[] animationSprites;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private float spriteChangeDelay = 0.2f;
    
    private Coroutine moveSpriteCoroutine;
    
    public override void OnNetworkSpawn()
    {
        _isMoving.OnValueChanged += HandleIsMovingChanged;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(!IsOwner) return;

        if (_playerInput == null)
        {
            _playerInput = new();
            _playerInput.Player.SetCallbacks(this);
        }
        _playerInput.Player.Enable();

        _rb = GetComponent<Rigidbody2D>();
        turretPivotTransform = transform.Find("PivotTurret");
        if (turretPivotTransform == null) Debug.LogError("PivotTurret is not found", gameObject);
        
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFireEvent.Invoke(true);
        }
        else if (context.canceled)
        {
            onFireEvent.Invoke(false);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _isMoving.Value = _moveInput.magnitude > 0.1f;
    }

    private void FixedUpdate()
    {
        if(!IsOwner) return;
        _rb.velocity = transform.up * (_moveInput.y * movementSpeed);
        _rb.MoveRotation(_rb.rotation + _moveInput.x * -shipRotationSpeed * Time.fixedDeltaTime);
    }
    private void LateUpdate()
    {
        if(!IsOwner) return;
        Vector2 screenToWorldPosition = Camera.main.ScreenToWorldPoint(_cursorLocation);
        Vector2 targetDirection = new Vector2(screenToWorldPosition.x - turretPivotTransform.position.x, screenToWorldPosition.y - turretPivotTransform.position.y).normalized;
        Vector2 currentDirection = Vector2.Lerp(turretPivotTransform.up, targetDirection, Time.deltaTime * turretRotationSpeed);
        turretPivotTransform.up = currentDirection;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        _cursorLocation = context.ReadValue<Vector2>();
    }
    
    
    private IEnumerator ChangeMovingSprite()
    {
        int movingSpriteIndex = 0;
        while (true)
        {
            _spriteRenderer.sprite = animationSprites[movingSpriteIndex];
            movingSpriteIndex = (movingSpriteIndex + 1) % animationSprites.Length;
            
            yield return new WaitForSeconds(spriteChangeDelay);
        }
    }
    
    public override void OnDestroy()
    {
        _isMoving.OnValueChanged -= HandleIsMovingChanged;
    }

    private void HandleIsMovingChanged(bool oldValue, bool newValue)
    {
        if (newValue && moveSpriteCoroutine == null)
        {
            moveSpriteCoroutine = StartCoroutine(ChangeMovingSprite());
        }
        else if (moveSpriteCoroutine != null)
        {
            StopCoroutine(moveSpriteCoroutine);
            moveSpriteCoroutine = null;
            _spriteRenderer.sprite = defaultSprite;
        }
    }
}
