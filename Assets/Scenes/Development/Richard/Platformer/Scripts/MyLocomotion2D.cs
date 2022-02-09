using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLocomotion2D : PhysicsObject2D, ILocomotion
{

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    IInputManager _inputManager;

    // Speed and smoothing
    [SerializeField] protected float _walkSpeed = 6f;
    [SerializeField] protected float _dashSpeed = 11f;
    [SerializeField] protected float _wallSlideSpeedMax = 3f;

    // Jumping
    [SerializeField] protected float _minJumpHeight = 1f;
    protected float _maxJumpVelocity;
    protected float _minJumpVelocity;

    // Wall Jump / Slide
    [SerializeField] protected float _wallStickTime = 0.25f;
    protected float _timeToWallUnstick;
    [SerializeField] protected Vector2 _wallJumpClimb;
    [SerializeField] protected Vector2 _wallJumpOff;
    [SerializeField] protected Vector2 _wallLeap;
    int wallDirectionX;
    bool isWallSliding;

    // Stamina
    Stamina stamina;
    [SerializeField] float _dashStaminaCost;
    [SerializeField] float _staminaIdleReplenishAmount;
    [SerializeField] float _staminaDrainFrequency = 0.1f;
    float staminaDrainTimer = 0;

    // Animation
    private MyAnimationStateManager _animationManager;



    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    protected override void Start()
    {
        base.Start();

        _inputManager = GetComponent<IInputManager>();
        stamina = GetComponent<Stamina>();

        _animationManager = transform.GetComponent<MyAnimationStateManager>();
        if (_animationManager == null)
        {
            Debug.LogError("IAnimationManager not found on " + name);
        }

        _collisions.FaceDirection = 1;
        _gravityStrength = -(2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
        _maxJumpVelocity = Mathf.Abs(_gravityStrength) * _timeToJumpApex;
        _minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravityStrength) * _minJumpHeight);
    }

    protected override void Update()
    {
        HorizontalMovement = _inputManager.Horizontal;
        VerticalMovement = _inputManager.Vertical;

        CalculateVelocity();
        HandleWallSliding();

        if (_velocity.x != 0)
        {
            HorizontalLook = _velocity.x > 0 ? 1 : -1;
            transform.localScale = new Vector3(HorizontalLook, 1, 1);
        }

        Dash();

        Move(_velocity * Time.deltaTime);

        if (_collisions.Above || _collisions.Below)
        {
            if (_collisions.SlidingDownMaxSlope)
            {
                _velocity.y += _collisions.SlopeNormal.y * -_gravityStrength * Time.deltaTime;
            }
            else
            {
                _velocity.y = 0;
            }
        }
        Stamina();
    }

    void Stamina()
    {
        if(Mathf.Abs(_velocity.x) <= Mathf.Epsilon && Mathf.Abs(_velocity.y) <= Mathf.Epsilon)
        {
            stamina.ReplenishStamina(_staminaIdleReplenishAmount);
        }
    }

    void Dash()
    {
        // Already dashing
        if(IsDashing && IsGrounded)
        {
            // Not moving
            if (Mathf.Abs(_velocity.x) <= Mathf.Epsilon)
            {
                IsDashCancelled = true;
                IsDashing = false;
                return;
            }
            if (_inputManager.Dash)
            {
                if (stamina == null || staminaDrainTimer < _staminaDrainFrequency)
                {
                    _velocity.x += _dashSpeed * HorizontalMovement;
                    staminaDrainTimer += Time.deltaTime;
                    return;
                }
                if (stamina.UseStamina(_dashStaminaCost))
                {
                    staminaDrainTimer = 0f; 
                    _velocity.x += _dashSpeed * HorizontalMovement;
                    return;
                }
                IsDashCancelled = true;
                IsDashing = false;
                return;
            }
        }

        if (_inputManager.Dash && IsGrounded && (stamina == null || stamina.UseStamina(_dashStaminaCost)))
        {
            IsDashing = true;
            _velocity.x += _dashSpeed * HorizontalMovement;
            return;
        }

        IsDashing = false;
    }

    protected override void CalculateVelocity()
    {
        float targetVelocityX = HorizontalMovement * _walkSpeed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, _collisions.Below ? _smoothTimeGrounded : _smoothTimeAirborne);
        _velocity.y += _gravityStrength * Time.deltaTime;
    }

    protected void HandleWallSliding()
    {
        wallDirectionX = (_collisions.Left) ? -1 : 1;
        isWallSliding = false;

        // Slide down wall
        if ((_collisions.Left || _collisions.Right) && !_collisions.Below && _velocity.y < 0)
        {
            isWallSliding = true;

            if (_velocity.y < -_wallSlideSpeedMax)
            {
                _velocity.y = -_wallSlideSpeedMax;
            }

            // Stick to wall for a short time to perform wall jumps easier
            if (_timeToWallUnstick > 0)
            {
                _velocityXSmoothing = 0;
                _velocity.x = 0;

                if (HorizontalMovement != wallDirectionX && HorizontalMovement != 0)
                {
                    _timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    _timeToWallUnstick = _wallStickTime;
                }
            }
            else
            {
                _timeToWallUnstick = _wallStickTime;
            }
        }

        if (_inputManager.Jump)
        {
            // Wall Jumps
            if (isWallSliding)
            {
                // Jump towards wall that you're sliding down
                if (wallDirectionX == HorizontalMovement)
                {
                    _velocity.x = -wallDirectionX * _wallJumpClimb.x;
                    _velocity.y = _wallJumpClimb.y;
                }
                // Jump off wall
                else if (HorizontalMovement == 0)
                {
                    _velocity.x = -wallDirectionX * _wallJumpOff.x;
                    _velocity.y = _wallJumpOff.y;
                }
                // Jump away from wall
                else
                {
                    _velocity.x = -wallDirectionX * _wallLeap.x;
                    _velocity.y = _wallLeap.y;
                }
            }

            // Jump from the ground
            if (_collisions.Below)
            {
                // Jump while sliding down a slope
                if (_collisions.SlidingDownMaxSlope)
                {
                    if (HorizontalMovement != -Mathf.Sign(_collisions.SlopeNormal.x))
                    {
                        _velocity.y = _maxJumpVelocity * _collisions.SlopeNormal.y;
                        _velocity.x = _maxJumpVelocity * _collisions.SlopeNormal.x;
                    }
                }
                // Normal jump
                else
                {
                    _velocity.y = _maxJumpVelocity;
                }
            }

            IsJumping = true;
        }

        if (_inputManager.CancelJump)
        {
            if (_velocity.y > _minJumpVelocity)
            {
                _velocity.y = _minJumpVelocity;
            }
        }
    }

    protected override void DetectHorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = _collisions.FaceDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + _skinWidth;

        if (Mathf.Abs(moveAmount.x) < _skinWidth)
        {
            rayLength = 2 * _skinWidth;
        }

        for (int i = 0; i < _horizontalRayCount; i++)
        {
            Vector2 rayOrigin;

            if (directionX == -1)
            {
                rayOrigin = _raycastOrigins.BottomLeft;
            }
            else
            {
                rayOrigin = _raycastOrigins.BottomRight;
            }

            rayOrigin += Vector2.up * (_horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            if (_showDebugLog)
            {
                Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);
            }

            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= _maxSlopeAngle)
                {
                    if (_collisions.DescendingSlope)
                    {
                        _collisions.DescendingSlope = false;
                        moveAmount = _collisions.PreviousVelocity;
                    }

                    float distanceToSlopeStart = 0;
                    if (slopeAngle != _collisions.PreviousSlopeAngle)
                    {
                        distanceToSlopeStart = hit.distance - _skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!_collisions.ClimbingSlope || slopeAngle > _maxSlopeAngle)
                {
                    if (moveAmount.x != 0)
                    {
                        moveAmount.x = (hit.distance - _skinWidth) * directionX;
                    }

                    rayLength = hit.distance;

                    if (_collisions.ClimbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(_collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    _collisions.Left = directionX == -1;
                    _collisions.Right = directionX == 1;
                }
            }
        }
    }

    protected void ClimbSlope(ref Vector2 velocity, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            _collisions.Below = true;
            _collisions.ClimbingSlope = true;
            _collisions.SlopeAngle = slopeAngle;
            _collisions.SlopeNormal = slopeNormal;
        }
    }

    protected void ResetFallingThroughPlatform()
    {
        _collisions.FallingThroughPlatform = false;
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public float HorizontalLook { get; set; } = 1;

    public float VerticalLook { get { throw new NotImplementedException(); } set { } }

    public bool IsJumping { get; set; }

    public bool IsJumpCancelled { get; set; }

    public bool IsDashing { get; set; }

    public bool IsDashCancelled { get; set; }

    public Vector2 Velocity { get { return _velocity; } }

    public bool IsWallSliding { get { return isWallSliding; } }

    public int WallDirectionX { get { return wallDirectionX; } }
}
