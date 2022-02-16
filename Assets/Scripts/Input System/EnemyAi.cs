using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class EnemyAi : MonoBehaviour, IInputManager
    {
        [SerializeField] protected bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Movement
        [SerializeField] protected Vector3[] _localWaypoints;
        private Vector3[] _globalWaypoints;
        private bool _isWaypointsSet = false;

        [SerializeField] protected float _speed;
        [SerializeField] protected bool _isCyclical;

        [SerializeField] protected float _waitTime;
        private float _nextMoveTime;

        private int _fromWaypointIndex;
        private float _distanceFromNextWaypoint;
        [SerializeField] private float _waypointErrorMargin = 0.1f;
        [SerializeField] private LayerMask _collisionLayerMask;

        // Combat
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private float _aggressiveRadius = 3;
        [SerializeField] private float _enemyErrorMargin = 1f;
        [SerializeField] private Transform _target;
        [SerializeField] private float _targetRange = 1f;

        [SerializeField] protected float _attackWaitTime;
        private float _nextAttackTime;

        private float _horizontal;
        private float _vertical;
        private bool _isJump;
        private bool _isJumpCancelled;
        private bool _isDash;
        private bool _isDashCancelled;
        private bool _isAttack;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        // Start is called before the first frame update
        private void Awake()
        {
            _globalWaypoints = new Vector3[_localWaypoints.Length];
            for (int i = 0; i < _globalWaypoints.Length; i++)
            {
                _globalWaypoints[i] = _localWaypoints[i] + transform.position;
            }

            _isWaypointsSet = _globalWaypoints.Length > 0;
        }

        // Update is called once per frame
        private void Update()
        {
            _isJump = false;
            _isAttack = false;

            DetermineAttackInput();
            DetermineHorizontalInput();

            if (Mathf.Abs(_horizontal) > 0)
            {
                DetermineIsJumping();
            }

            if (_showDebugLog)
            {
                Debug.Log("Horizontal: " + _horizontal);
                Debug.Log("Vertical: " + _vertical);
                Debug.Log("Jumping: " + _isJump);
                Debug.Log("Attacking: " + _isAttack);
            }
        }

        private void DetermineAttackInput()
        {
            if (Time.time < _nextAttackTime)
            {
                return;
            }

            // Is player in attacking range?
            Collider2D enemy = Physics2D.OverlapCircle(_target.position, _targetRange, _enemyLayerMask);
            if (enemy != null)
            {
                // Attack if applicable
                _isAttack = true;
                _nextAttackTime = Time.time + _attackWaitTime;
            }
        }

        private void DetermineHorizontalInput()
        {
            TargetInfo targetInfo = GetTargetPositionAndErrorMargin();

            if (targetInfo.IsEnemy == false && Time.time < _nextMoveTime)
            {
                if (_showDebugLog)
                {
                    Debug.Log("Horizontal input on " + name + " is set to zero. Current time is less than next move time.");
                }

                _horizontal = 0;
                return;
            }

            // Get distance between current position and target
            float _distanceFromTargetPosition = Vector3.Distance(transform.position, targetInfo.Position);

            // If arrived at next way point get the next way point
            if (_distanceFromTargetPosition <= targetInfo.ErrorMargin)
            {
                if (_showDebugLog)
                {
                    Debug.Log("Horizontal input on " + name + " is set to zero. Distance from target position is within target error margin.");
                }

                _horizontal = 0;
                return;
            }

            // Set horizontal movement and facing based on target location
            _horizontal = Mathf.Sign(targetInfo.Position.x - transform.position.x);
        }

        private TargetInfo GetTargetPositionAndErrorMargin()
        {
            // Is player nearby?
            Collider2D enemy = Physics2D.OverlapCircle(transform.position, _aggressiveRadius, _enemyLayerMask);
            if (enemy != null)
            {
                if (_showDebugLog)
                {
                    Debug.Log(name + " has targetted " + enemy.name + " as an enemy.");
                }

                return new TargetInfo(enemy.transform.position, _enemyErrorMargin, true);
            }

            if (_isWaypointsSet == false)
            {
                return new TargetInfo(transform.position, 1f);
            }

            _fromWaypointIndex %= _globalWaypoints.Length;
            int toWayPointIndex = (_fromWaypointIndex + 1) % _globalWaypoints.Length;

            // Arrived at next waypoint?
            _distanceFromNextWaypoint = Vector3.Distance(transform.position, _globalWaypoints[toWayPointIndex]);
            if (_distanceFromNextWaypoint <= _waypointErrorMargin)
            {
                // Increment the waypoint index
                _fromWaypointIndex++;

                // Reverse the list of waypoints of _isCyclical is false
                if (!_isCyclical)
                {
                    if (_fromWaypointIndex >= _globalWaypoints.Length - 1)
                    {
                        _fromWaypointIndex = 0;
                        System.Array.Reverse(_globalWaypoints);
                    }
                }

                // Set time when the game object can move again
                _nextMoveTime = Time.time + _waitTime;
            }

            return new TargetInfo(_globalWaypoints[toWayPointIndex], _waypointErrorMargin);
        }

        private void DetermineIsJumping()
        {
            // Is colliding with object in facing direction?
            Collider2D collider = Physics2D.OverlapCircle(_target.position, _targetRange, _collisionLayerMask);
            _isJump = collider == null ? false : true;
        }

        private void OnDrawGizmos()
        {
            if (_localWaypoints != null)
            {
                Gizmos.color = Color.red;
                float size = 0.3f;

                for (int i = 0; i < _localWaypoints.Length; i++)
                {
                    Vector3 globalWaypointPosition = (Application.isPlaying) ? _globalWaypoints[i] : _localWaypoints[i] + transform.position;
                    Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
                    Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);

                }
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _aggressiveRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_target.position, _targetRange);
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        private struct TargetInfo
        {
            public TargetInfo(Vector2 position, float errorMargin, bool isEnemy = false)
            {
                Position = position;
                ErrorMargin = errorMargin;
                IsEnemy = isEnemy;
            }

            public Vector2 Position;
            public float ErrorMargin;
            public bool IsEnemy;
        }

        public float Horizontal { get { return _horizontal; } }

        public float Vertical { get { return _vertical; } }

        public bool IsJump { get { return _isJump; } }

        public bool IsJumpCancelled { get { return _isJumpCancelled; } }

        public bool IsDash { get { return _isDash; } }

        public bool IsDashCancelled { get { return _isDashCancelled; } }

        public bool IsAttack { get { return _isAttack; } }
    }
}
