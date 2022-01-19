using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    [SerializeField] private LayerMask _passengerMask;
    [SerializeField] private Vector3[] _localWaypoints;
    private Vector3[] _globalWaypoints;

    [SerializeField] private float _speed;
    [SerializeField] private bool _isCyclical;

    [SerializeField] private float _waitTime;
    private float _nextMoveTime;

    [Range(0, 2)]
    [SerializeField] private float _easeAmount;

    private int _fromWaypointIndex;
    private float _percentBetweenWaypoints;

    private List<PassengerMovement> _passengerMovement;
    private Dictionary<Transform, Locomotion2d> _passengerDictionary = new Dictionary<Transform, Locomotion2d>();

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    protected override void Awake()
    {
        base.Awake();

        _globalWaypoints = new Vector3[_localWaypoints.Length];
        for(int i = 0; i < _globalWaypoints.Length; i++)
        {
            _globalWaypoints[i] = _localWaypoints[i] + transform.position;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRacastOrigins();
        Vector3 velocity = CalculatePlatformMovement();

        CalculatePassengerMovement(velocity);
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    private void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        _passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertical movement
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + _skinWidth;

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

                rayOrigin += Vector2.right * (_verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, _passengerMask);
                
                if (hit && hit.distance != 0)
                {

                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - _skinWidth) * directionY;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        // Horizontal movement
        if(velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + _skinWidth;

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
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _passengerMask);
                
                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x - (hit.distance - _skinWidth) * directionX;
                        float pushY = -_skinWidth;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        // Passenger on top of a horizontally or downward moving platform
        if(directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = _skinWidth * 2;

            for (int i = 0; i < _verticalRayCount; i++)
            {
                Vector2 rayOrigin = _raycastOrigins.TopLeft + Vector2.right * (_verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, _passengerMask);

                if (hit && hit.distance != 0)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        _passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    private void MovePassengers(bool beforeMovePlatform)
    {
        foreach(PassengerMovement passenger in _passengerMovement)
        {
            if (!_passengerDictionary.ContainsKey(passenger.Transform))
            {
                _passengerDictionary.Add(passenger.Transform, passenger.Transform.GetComponent<Locomotion2d>());
            }

            if(passenger.MoveBeforePlatform == beforeMovePlatform)
            {
                _passengerDictionary[passenger.Transform].Move(passenger.Velocity, passenger.IsStandingOnPlatform);
            }
        }
    }

    private Vector3 CalculatePlatformMovement()
    {
        if(Time.time < _nextMoveTime)
        {
            return Vector3.zero;
        }

        _fromWaypointIndex %= _globalWaypoints.Length;
        int toWayPointIndex = (_fromWaypointIndex + 1) % _globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(_globalWaypoints[_fromWaypointIndex], _globalWaypoints[toWayPointIndex]);
        _percentBetweenWaypoints += Time.deltaTime * _speed / distanceBetweenWaypoints;
        _percentBetweenWaypoints = Mathf.Clamp01(_percentBetweenWaypoints);
        float easedPercentBetweenWayPoints = Ease(_percentBetweenWaypoints);

        Vector3 newPosition = Vector3.Lerp(_globalWaypoints[_fromWaypointIndex], _globalWaypoints[toWayPointIndex], easedPercentBetweenWayPoints);

        if(_percentBetweenWaypoints >= 1)
        {
            _percentBetweenWaypoints = 0;
            _fromWaypointIndex++;

            if (!_isCyclical)
            {
                if (_fromWaypointIndex >= _globalWaypoints.Length - 1)
                {
                    _fromWaypointIndex = 0;
                    System.Array.Reverse(_globalWaypoints);
                }
            }

            _nextMoveTime = Time.time + _waitTime;
        }

        return newPosition - transform.position;
    }

    private float Ease(float x)
    {
        float a = _easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    private void OnDrawGizmos()
    {
        if(_localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = 0.3f;

            for(int i = 0; i < _localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPosition = (Application.isPlaying) ? _globalWaypoints[i] : _localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);

            }
        }
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    private struct PassengerMovement
    {
        public Transform Transform;
        public Vector3 Velocity;
        public bool IsStandingOnPlatform;
        public bool MoveBeforePlatform;

        public PassengerMovement(Transform transform, Vector3 velocity, bool isStandingOnPlatform, bool moveBeforePlatform)
        {
            Transform = transform;
            Velocity = velocity;
            IsStandingOnPlatform = isStandingOnPlatform;
            MoveBeforePlatform = moveBeforePlatform;
        }
    }
}
