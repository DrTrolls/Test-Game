using UnityEngine;

/// <summary>
/// Handles 2D top-down player movement with smooth acceleration/deceleration.
/// Uses the default Input Manager axes (Horizontal/Vertical) and WASD/arrow keys.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float acceleration = 18f;
    [SerializeField] private float deceleration = 22f;
    [SerializeField] private float inputDeadZone = 0.1f;
    [SerializeField] private float maxSpeed = 8f;

    private Rigidbody2D _rigidbody;
    private Vector2 _input;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _input = new Vector2(horizontal, vertical);

        if (_input.magnitude < inputDeadZone)
        {
            _input = Vector2.zero;
        }
        else
        {
            _input = _input.normalized;
        }
    }

    private void FixedUpdate()
    {
        Vector2 desiredVelocity = _input * moveSpeed;
        Vector2 currentVelocity = _rigidbody.velocity;

        float chosenAcceleration = _input == Vector2.zero ? deceleration : acceleration;
        Vector2 velocityChange = desiredVelocity - currentVelocity;

        float maxDelta = chosenAcceleration * Time.fixedDeltaTime;
        velocityChange = Vector2.ClampMagnitude(velocityChange, maxDelta);

        Vector2 newVelocity = currentVelocity + velocityChange;
        newVelocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
        _rigidbody.velocity = newVelocity;
    }
}
