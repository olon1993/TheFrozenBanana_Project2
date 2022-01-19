using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion2d : RaycastController, ILocomotion
{

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Speed and smoothing
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float _walkSpeed = 6f;
    [SerializeField] private float _dashSpeed = 11f;
    [SerializeField] private float _wallSlideSpeedMax = 3f;
    private float _velocityXSmoothing;
    private float _smoothTimeAirborne = 0.1f;
    private float _smoothTimeGrounded = 0f;

    // Collisions
    private CollisionInfo _collisions;

    // Gravity
    private float _gravityStrength = -2.98f;

    // Jumping
    [SerializeField] private float _maxJumpHeight = 2f;
    [SerializeField] private float _minJumpHeight = 1f;
    [SerializeField] private float _timeToJumpApex = 0.4f;
    private float _maxJumpVelocity;
    private float _minJumpVelocity;

    // Wall Jump / Slide
    [SerializeField] private float _wallStickTime = 0.25f;
    private float _timeToWallUnstick;
    [SerializeField] private Vector2 _wallJumpClimb;
    [SerializeField] private Vector2 _wallJumpOff;
    [SerializeField] private Vector2 _wallLeap;

    // Climbing
    [SerializeField] private float _maxSlopeAngle = 80;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        _collisions.FaceDirection = 1;
        _gravityStrength = -(2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
        _maxJumpVelocity = Mathf.Abs(_gravityStrength) * _timeToJumpApex;
        _minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravityStrength) * _minJumpHeight);
    }

    private void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        if (IsDashing)
        {
            Debug.Log("Dash");
        }

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
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = HorizontalMovement * _walkSpeed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, _collisions.Below ? _smoothTimeGrounded : _smoothTimeAirborne);
        _velocity.y += _gravityStrength * Time.deltaTime;
    }

    private void HandleWallSliding()
    {
        int wallDirectionX = (_collisions.Left) ? -1 : 1;
        bool isWallSliding = false;
        if ((_collisions.Left || _collisions.Right) && !_collisions.Below && _velocity.y < 0)
        {
            isWallSliding = true;

            if (_velocity.y < -_wallSlideSpeedMax)
            {
                _velocity.y = -_wallSlideSpeedMax;
            }

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

        if (IsJumping)
        {
            if (isWallSliding)
            {
                if (wallDirectionX == HorizontalMovement)
                {
                    _velocity.x = -wallDirectionX * _wallJumpClimb.x;
                    _velocity.y = _wallJumpClimb.y;
                }
                else if (HorizontalMovement == 0)
                {
                    _velocity.x = -wallDirectionX * _wallJumpOff.x;
                    _velocity.y = _wallJumpOff.y;
                }
                else
                {
                    _velocity.x = -wallDirectionX * _wallLeap.x;
                    _velocity.y = _wallLeap.y;
                }
            }

            if (_collisions.Below)
            {
                if (_collisions.SlidingDownMaxSlope)
                {
                    if(HorizontalMovement != -Mathf.Sign(_collisions.SlopeNormal.x))
                    {
                        _velocity.y = _maxJumpVelocity * _collisions.SlopeNormal.y;
                        _velocity.x = _maxJumpVelocity * _collisions.SlopeNormal.x;
                    }
                }
                else
                {
                    _velocity.y = _maxJumpVelocity;
                }
            }
        }

        if (IsJumpCancelled)
        {
            if (_velocity.y > _minJumpVelocity)
            {
                _velocity.y = _minJumpVelocity;
            }
        }
    }

    public void Move(Vector2 moveAmount, bool isStandingOnPlatform = false)
    {
        UpdateRacastOrigins();
        _collisions.Reset();
        _collisions.PreviousVelocity = moveAmount;

        if(moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        if (moveAmount.x != 0)
        {
            _collisions.FaceDirection = (int)Mathf.Sign(moveAmount.x);
        }

        DetectHorizontalCollisions(ref moveAmount);
        
        if(moveAmount.y != 0)
        {
            DetectVerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);

        if (isStandingOnPlatform)
        {
            _collisions.Below = true;
        }
    }

    private void DetectHorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = _collisions.FaceDirection;
        float rayLength = Mathf.Abs(moveAmount.x) + _skinWidth;

        if(Mathf.Abs(moveAmount.x) < _skinWidth)
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

                if(i == 0 && slopeAngle <= _maxSlopeAngle)
                {
                    if (_collisions.DescendingSlope)
                    {
                        _collisions.DescendingSlope = false;
                        moveAmount = _collisions.PreviousVelocity;
                    }

                    float distanceToSlopeStart = 0;
                    if(slopeAngle != _collisions.PreviousSlopeAngle)
                    {
                        distanceToSlopeStart = hit.distance - _skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if(!_collisions.ClimbingSlope || slopeAngle > _maxSlopeAngle)
                {
                    moveAmount.x = (hit.distance - _skinWidth) * directionX;
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

    private void DetectVerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + _skinWidth;

        for (int i = 0; i < _verticalRayCount; i++)
        {
            Vector2 rayOrigin;

            if(directionY == -1)
            {
                rayOrigin = _raycastOrigins.BottomLeft;
            }
            else
            {
                rayOrigin = _raycastOrigins.TopLeft;
            }

            rayOrigin += Vector2.right * (_verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

            if (_showDebugLog)
            {
                Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
            }

            if (hit)
            {
                if (hit.collider.CompareTag("PassableObstacle"))
                {
                    if(directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }

                    if (_collisions.FallingThroughPlatform)
                    {
                        continue;
                    }

                    if(VerticalMovement == -1)
                    {
                        _collisions.FallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.25f);
                        continue;
                    }
                }

                _collisions.Below = directionY == -1;
                _collisions.Above = directionY == 1;

                if (_collisions.ClimbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(_collisions.SlopeAngle * Mathf.Rad2Deg) * Mathf.Sign(moveAmount.x);
                }

                moveAmount.y = (hit.distance - _skinWidth) * directionY;
                rayLength = hit.distance;
            }
        }

        if (_collisions.ClimbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + _skinWidth;
            Vector2 rayOrigin;

            if(directionX == -1)
            {
                rayOrigin = _raycastOrigins.BottomLeft;
            }
            else
            {
                rayOrigin = _raycastOrigins.BottomRight;
            }

            rayOrigin += Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != _collisions.SlopeAngle)
                {
                    moveAmount.x = (hit.distance - _skinWidth) * directionX;
                    _collisions.SlopeAngle = slopeAngle;
                    _collisions.SlopeNormal = hit.normal;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 velocity, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if(velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            _collisions.Below = true;
            _collisions.ClimbingSlope = true;
            _collisions.SlopeAngle = slopeAngle;
            _collisions.SlopeNormal = slopeNormal;
        }
    }

    private void DescendSlope(ref Vector2 moveAmount)
    {
        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(_raycastOrigins.BottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + _skinWidth, CollisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(_raycastOrigins.BottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + _skinWidth, CollisionMask);

        if(maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }

        if (!_collisions.SlidingDownMaxSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            Vector2 rayOrigin;

            if (directionX == -1)
            {
                rayOrigin = _raycastOrigins.BottomRight;
            }
            else
            {
                rayOrigin = _raycastOrigins.BottomLeft;
            }

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, CollisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= _maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - _skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                        {
                            float moveDistance = Mathf.Abs(moveAmount.x);
                            float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= descendVelocityY;

                            _collisions.SlopeAngle = slopeAngle;
                            _collisions.DescendingSlope = true;
                            _collisions.Below = true;
                            _collisions.SlopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    private void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
    {
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle > _maxSlopeAngle)
            {
                moveAmount.x = hit.normal.x * ((Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad));

                _collisions.SlopeAngle = slopeAngle;
                _collisions.SlidingDownMaxSlope = true;
                _collisions.SlopeNormal = hit.normal;
            }
        }
    }

    private void ResetFallingThroughPlatform()
    {
        _collisions.FallingThroughPlatform = false;
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public struct CollisionInfo
    {
        public bool Above, Below, Left, Right, ClimbingSlope, DescendingSlope, SlidingDownMaxSlope, FallingThroughPlatform;
        public float SlopeAngle, PreviousSlopeAngle;
        public Vector2 PreviousVelocity, SlopeNormal;
        public int FaceDirection;

        public void Reset()
        {
            Above = Below = Left = Right = ClimbingSlope = DescendingSlope = SlidingDownMaxSlope = false;
            PreviousSlopeAngle = SlopeAngle;
            SlopeAngle = 0;
            SlopeNormal = Vector2.zero;
        }
    }

    public float HorizontalMovement { get; set; }

    public float VerticalMovement { get; set; }

    public Vector3 Movement { get { return new Vector3(HorizontalMovement, VerticalMovement); } }

    public float HorizontalLook { get { throw new NotImplementedException(); } set { } }

    public float VerticalLook { get { throw new NotImplementedException(); } set { } }

    public bool IsJumping { get; set; }

    public bool IsJumpCancelled { get; set; }

    public bool IsGrounded { get { return _collisions.Below; } }

    public bool IsDashing { get; set; }
}
