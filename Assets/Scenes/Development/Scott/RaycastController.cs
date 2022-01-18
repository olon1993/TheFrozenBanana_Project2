using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    [SerializeField] protected bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    [SerializeField] protected LayerMask CollisionMask;
    protected BoxCollider2D _collider;
    protected RaycastOrigins _raycastOrigins;
    protected float _horizontalRaySpacing;
    protected float _verticalRaySpacing;
    protected const float _distanceBetweenRays = 0.25f;
    protected int _horizontalRayCount;
    protected int _verticalRayCount;
    protected const float _skinWidth = 0.015f;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    protected virtual void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("BoxCollider2D not found on " + name);
        }
    }

    protected virtual void Start()
    {
        CalculateRaySpacing();
    }

    protected void UpdateRacastOrigins()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(_skinWidth * -2);

        _raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        _raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
        _raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
        _raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    protected void CalculateRaySpacing()
    {
        Bounds bounds = _collider.bounds;
        bounds.Expand(_skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        _horizontalRayCount = Mathf.RoundToInt(boundsHeight / _distanceBetweenRays);
        _verticalRayCount = Mathf.RoundToInt(boundsWidth / _distanceBetweenRays);

        _horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
        _verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    protected struct RaycastOrigins
    {
        public Vector2 TopRight, TopLeft;
        public Vector2 BottomRight, BottomLeft;
    }
}
