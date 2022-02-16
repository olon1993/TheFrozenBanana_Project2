using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class PhysicsObject2D : RaycastController
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Speed and smoothing
        protected Vector3 _velocity = Vector3.zero;
        protected float _velocityXSmoothing;
        protected float _smoothTimeAirborne = 0.1f;
        protected float _smoothTimeGrounded = 0f;

        // Collisions
        protected CollisionInfo _collisions;

        // Gravity
        protected float _gravityStrength = -2.98f;

        // Jumping
        [SerializeField] protected float _maxJumpHeight = 2f;
        [SerializeField] protected float _timeToJumpApex = 0.4f;

        // Climbing
        [SerializeField] protected float _maxSlopeAngle = 80;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Start()
        {
            base.Start();

            _collisions.FaceDirection = 1;
            _gravityStrength = -(2 * _maxJumpHeight) / Mathf.Pow(_timeToJumpApex, 2);
        }

        protected virtual void Update()
        {
            CalculateVelocity();
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

        protected virtual void CalculateVelocity()
        {
            float targetVelocityX = HorizontalMovement;
            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, _collisions.Below ? _smoothTimeGrounded : _smoothTimeAirborne);
            _velocity.y += _gravityStrength * Time.deltaTime;
        }

        public void Move(Vector2 moveAmount, bool isStandingOnPlatform = false)
        {

            UpdateRacastOrigins();
            _collisions.Reset();
            _collisions.PreviousVelocity = moveAmount;

            if (moveAmount.y < 0)
            {
                DescendSlope(ref moveAmount);
            }

            if (moveAmount.x != 0)
            {
                _collisions.FaceDirection = (int)Mathf.Sign(moveAmount.x);
            }

            DetectHorizontalCollisions(ref moveAmount);

            if (moveAmount.y != 0)
            {
                DetectVerticalCollisions(ref moveAmount);
            }

            transform.Translate(moveAmount);

            if (isStandingOnPlatform)
            {
                _collisions.Below = true;
            }
        }

        protected virtual void DetectHorizontalCollisions(ref Vector2 moveAmount)
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
                        moveAmount.x += distanceToSlopeStart * directionX;
                    }

                    if (!_collisions.ClimbingSlope || slopeAngle > _maxSlopeAngle)
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

        protected virtual void DetectVerticalCollisions(ref Vector2 moveAmount)
        {
            float directionY = Mathf.Sign(moveAmount.y);
            float rayLength = Mathf.Abs(moveAmount.y) + _skinWidth;

            for (int i = 0; i < _verticalRayCount; i++)
            {
                Vector2 rayOrigin;

                if (directionY == -1)
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
                        if (directionY == 1 || hit.distance == 0)
                        {
                            continue;
                        }

                        if (_collisions.FallingThroughPlatform)
                        {
                            continue;
                        }

                        if (VerticalMovement == -1)
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

                if (directionX == -1)
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
                    if (slopeAngle != _collisions.SlopeAngle)
                    {
                        moveAmount.x = (hit.distance - _skinWidth) * directionX;
                        _collisions.SlopeAngle = slopeAngle;
                        _collisions.SlopeNormal = hit.normal;
                    }
                }
            }
        }

        protected void DescendSlope(ref Vector2 moveAmount)
        {
            RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(_raycastOrigins.BottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + _skinWidth, CollisionMask);
            RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(_raycastOrigins.BottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + _skinWidth, CollisionMask);

            if (maxSlopeHitLeft ^ maxSlopeHitRight)
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

        protected void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
        {
            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle > _maxSlopeAngle)
                {
                    moveAmount.x = hit.normal.x * ((Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad));

                    _collisions.SlopeAngle = slopeAngle;
                    _collisions.SlidingDownMaxSlope = true;
                    _collisions.SlopeNormal = hit.normal;
                }
            }
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

        public Vector3 Velocity { get { return _velocity; } }

        public bool IsGrounded { get { return _collisions.Below; } }

        public bool IsRightCollision { get { return _collisions.Right; } }

        public bool IsLeftCollision { get { return _collisions.Left; } }
    }
}
