using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _groundLinearDrag;
    private float _horizontalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _airLienarDrag = 2.5f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private float _hangTime = .1f;
    [SerializeField] private float _jumpBufferLength = .1f;
    private float _hangTimeCounter;
    private float _jumpBufferCounter;
    private bool _canJump => _jumpBufferCounter > 0 && (_hangTimeCounter > 0f || _onWall);
    private bool _isJumping = false;

    [Header("Wall Movement Variables")]
    [SerializeField] private float _wallSlideModifier = 0.5f;
    [SerializeField] private float _wallJumpXVelocityHaltDelay = 0.2f;
    private bool _wallGrab => _onWall && !_onGround;
    private bool _wallSlide => _onWall && !_onGround && _wallGrab;

    [Header("Ground Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private Vector3 _groundRaycastOffset;
    private bool _onGround;

    [Header("Wall Collision Variables")]
    [SerializeField] private float _wallRaycastLength;
    private bool _onWall;
    private bool _onRightWall;

    [Header("Corner Correction Variables")]
    [SerializeField] private float _topRaycastLength;
    [SerializeField] Vector3 _edgeRaycastOffset;
    [SerializeField] Vector3 _innerRaycastOffset;
    private bool _canCornerCorrect;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _horizontalDirection = GetInput().x;
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _jumpBufferLength;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_canJump)
        {
            if (_onWall && !_onGround)
            {
                if (_onRightWall && _horizontalDirection > 0f || !_onRightWall && _horizontalDirection < 0f)
                {
                    StartCoroutine(NeutralWallJump());
                }
                else
                {
                    WallJump();
                }
            }
            else
            {
                Jump(Vector2.up);
            }
        }
    }


    private void FixedUpdate()
    {
        CheckCollisions();
        MoveCharacter();
        if (_onGround)
        {
            ApplyGroundLinearDrag();
            _hangTimeCounter = _hangTime;
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
            _hangTimeCounter -= Time.fixedDeltaTime;
            if (!_onWall || _rb.velocity.y < 0f) _isJumping = false;
        }
        if (_canCornerCorrect) CornerCorrect(_rb.velocity.y);
        if (!_isJumping)
        {
            if (_wallGrab) WallGrab();
            if (_wallSlide) WallSlide();
            if (_onWall) StickToWall();
        }
    }

    private Vector2 GetInput()
    {
        /* Creates a 2D vector from WASD input */
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }


    private void MoveCharacter()
    {
        /* Applies a force to rigidbody to move it horizontally, checks if max velocity is reached
           and corrects it to the max value */
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
    }

    private void ApplyGroundLinearDrag()
    {
        /* Applies ground drag to rigidbody if its velocity is less than 0.4 or it changes direction */
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _groundLinearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        /* Applies air linear drag to rigodbody */
        _rb.drag = _airLienarDrag;
    }

    private void Jump(Vector2 direction)
    {
        /* Apply a force to rigidbody to simulate jumping, sets hang and jump buffr counters to zero */
        ApplyAirLinearDrag();
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(direction * _jumpForce, ForceMode2D.Impulse);

        _hangTimeCounter = 0f;
        _jumpBufferCounter = 0f;
        _isJumping = true;
    }

    private void WallJump()
    {
        /* Wall jump, jumps opposite of the wall */
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
    }

    IEnumerator NeutralWallJump()
    {
        /* Neutral wall jump, acts as if normal jump is done on a wall by introducing a coroutine delay */
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
        yield return new WaitForSeconds(_wallJumpXVelocityHaltDelay);
        _rb.velocity = new Vector2(0f, _rb.velocity.y);
    }

    void CornerCorrect(float Yvelocity)
    {
        /* When the player is a few pixels under an obstacle, raycasts will detect it and the player will 
         * be moved in the appropriate direction by those few pixels. Resulting velocity vector will also 
         * be adjusted to match jumping from the corrected position. This prevents the player from bumping 
         * into the ceiling by a few pixels and provides a smoother movement experience. */

        RaycastHit2D _hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.left, _topRaycastLength, _groundLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x + _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
            return;
        }

        _hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.right, _topRaycastLength, _groundLayer);
        if (_hit.collider != null)
        {
            float _newPos = Vector3.Distance(new Vector3(_hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x - _newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, Yvelocity);
        }
    }

    private void CheckCollisions()
    {
        /* Raycasts that check 1) collisions with the ground, 2) corners of platforms above the players and 3) walls */
        // 1) Ground Collisions
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer);

        // 2) Corner Collisions
        _canCornerCorrect = Physics2D.Raycast(transform.position + _edgeRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) &&
                            !Physics2D.Raycast(transform.position + _innerRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) ||
                            Physics2D.Raycast(transform.position - _edgeRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) &&
                            !Physics2D.Raycast(transform.position - _innerRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer);

        // 3) Wall Collisions
        _onWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer) ||
                  Physics2D.Raycast(transform.position, Vector2.left, _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer);
    }

    private void OnDrawGizmos()
    {
        /* Draws gizmos for the raycasts */
        Gizmos.color = Color.green;

        //Ground check
        Gizmos.DrawLine(transform.position + _groundRaycastOffset, transform.position + _groundRaycastOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundRaycastOffset, transform.position - _groundRaycastOffset + Vector3.down * _groundRaycastLength);

        //Corner check
        Gizmos.DrawLine(transform.position + _edgeRaycastOffset, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _edgeRaycastOffset, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset, transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _innerRaycastOffset, transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength);

        //Corner distance check
        Gizmos.DrawLine(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength,
                        transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.left * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength,
                        transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.right * _topRaycastLength);

        //Wall check
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _wallRaycastLength);
    }

    private void FallMultiplier()
    {
        /* Applies a multiplier to gravity of rigidbody, simulates acceleration */
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _fallMultiplier;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rb.gravityScale = 1f;
        }
    }

    void WallGrab()
    {
        /* Ensures that rigidbody sticks to wall when it collides with it by setting y component of vector to 0*/
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
    }

    void WallSlide()
    {
        /* Rigidbody begins to slide down the wall with a specified modifier*/
        _rb.velocity = new Vector2(_rb.velocity.x, -_maxMoveSpeed * _wallSlideModifier);
    }

    void StickToWall()
    {
        /* Ensures that rigidbody sticks to the wall when wall raycast detects collsion that is a few pixels off
           by moving rigidbody in the correct direction */
        if (_onRightWall && _horizontalDirection >= 0)
        {
            _rb.velocity = new Vector2(1f, _rb.velocity.y);
        }
        else if (!_onRightWall && _horizontalDirection <= 0f)
        {
            _rb.velocity = new Vector2(-1f, _rb.velocity.y);
        }
    }
}