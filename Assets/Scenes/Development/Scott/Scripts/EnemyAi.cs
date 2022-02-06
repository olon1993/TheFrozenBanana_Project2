using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private ILocomotion _locomotion;
    private IAnimationManager _animationManager;
    private ICombatant _combatant;

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

    // Combat
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float _aggressiveRadius = 3;
    [SerializeField] private float _enemyErrorMargin = 1f;

    [SerializeField] protected float _attackWaitTime;
    private float _nextAttackTime;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    // Start is called before the first frame update
    void Awake()
    {
        _locomotion = transform.GetComponent<ILocomotion>();
        if (_locomotion == null)
        {
            Debug.LogError("ILocomotion not found on " + name);
        }

        _combatant = transform.GetComponent<ICombatant>();
        if (_combatant == null)
        {
            Debug.LogError("ICombatant not found on " + name);
        }

        _animationManager = transform.GetComponent<IAnimationManager>();
        if (_animationManager == null)
        {
            Debug.LogError("IAnimationManager not found on " + name);
        }

        _globalWaypoints = new Vector3[_localWaypoints.Length];
        for (int i = 0; i < _globalWaypoints.Length; i++)
        {
            _globalWaypoints[i] = _localWaypoints[i] + transform.position;
        }

        _isWaypointsSet = _globalWaypoints.Length > 0;
    }

    // Update is called once per frame
    void Update()
    {
        Reset();

        DetermineAttackInput();
        DetermineHorizontalInput();

        if (_locomotion.HorizontalMovement != 0)
        {
            DetermineIsJumping();
        }

        if (_showDebugLog)
        {
            Debug.Log("Horizontal Look: " + _locomotion.HorizontalLook);
            Debug.Log("Right Collision: " + _locomotion.IsRightCollision);
            Debug.Log("Left Collision: " + _locomotion.IsLeftCollision);
            Debug.Log("Jumping: " + _locomotion.IsJumping);
            Debug.Log("Attacking: " + _combatant.IsAttacking);
        }
    }

    private void Reset()
    {
        _locomotion.IsJumping = false;
        _combatant.IsAttacking = false;
    }

    private void DetermineAttackInput()
    {
        if(Time.time < _nextAttackTime)
        {
            return;
        }

        // Is player in attacking range?
        if (_combatant.CurrentWeapon is IMeleeWeapon)
        {
            Collider2D enemy = Physics2D.OverlapCircle(_combatant.CurrentWeapon.PointOfOrigin.position, (_combatant.CurrentWeapon as IMeleeWeapon).AttackRange, _enemyLayerMask);
            if (enemy != null)
            {
                // Attack if applicable
                _combatant.IsAttacking = true;
                _nextAttackTime = Time.time + _attackWaitTime;
            }
        }
    }

    private void DetermineHorizontalInput()
    {
        TargetInfo targetInfo = GetTargetPositionAndErrorMargin();

        if (Time.time < _nextMoveTime)
        {
            if (_showDebugLog)
            {
                Debug.Log("Horizontal input on " + name + " is set to zero. Current time is less than next move time.");
            }


            if (_locomotion.HorizontalLook > 0)
            {
                _animationManager.RequestStateChange(AnimationState.IDLE_RIGHT);
            }
            else
            {
                _animationManager.RequestStateChange(AnimationState.IDLE_LEFT);
            }

            _locomotion.HorizontalMovement = 0;
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

            if (_locomotion.HorizontalLook > 0)
            {
                _animationManager.RequestStateChange(AnimationState.IDLE_RIGHT);
            }
            else
            {
                _animationManager.RequestStateChange(AnimationState.IDLE_LEFT);
            }

            _locomotion.HorizontalMovement = 0;
            return;
        }

        // Set horizontal movement and facing based on target location
        float horizontal = Mathf.Clamp(targetInfo.Position.x - transform.position.x, -1, 1);
        _combatant.HorizontalFacingDirection = (int)horizontal;
        _locomotion.HorizontalMovement = horizontal;

        if (horizontal > 0)
        {
            _animationManager.RequestStateChange(AnimationState.WALK_RIGHT);
        }
        else
        {
            _animationManager.RequestStateChange(AnimationState.WALK_LEFT);
        }
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

            return new TargetInfo(enemy.transform.position, _enemyErrorMargin);
        }

        if(_isWaypointsSet == false)
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
        if ((_locomotion.IsRightCollision && _locomotion.HorizontalLook == 1) || (_locomotion.IsLeftCollision && _locomotion.HorizontalLook == -1))
        {
            // Jump
            _locomotion.IsJumping = true;
        }
    }

    protected void OnDrawGizmos()
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
    }

    private struct TargetInfo
    {
        public TargetInfo(Vector2 position, float errorMargin)
        {
            Position = position;
            ErrorMargin = errorMargin;
        }

        public Vector2 Position;
        public float ErrorMargin;
    }
}
