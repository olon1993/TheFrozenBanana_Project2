using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _stimulusMask;
    [SerializeField] private GameObject _toEnable;
    [SerializeField] private Vector2 _size;
    [SerializeField] private bool _destroyOnEnable;

    private void Update()
    {
        if (_toEnable.activeInHierarchy)
        {
            return;
        }

        Collider2D _stimuli = Physics2D.OverlapArea(transform.position, new Vector2(transform.position.x + _size.x, transform.position.y + _size.y), _stimulusMask);
        if (_stimuli != null) 
        { 
            _toEnable.SetActive(true);
            if (_destroyOnEnable)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + _size.x, transform.position.y));
        Gizmos.DrawLine(new Vector2(transform.position.x + _size.x, transform.position.y), new Vector2(transform.position.x + _size.x, transform.position.y + _size.y));
        Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y + _size.y), new Vector2(transform.position.x + _size.x, transform.position.y + _size.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + _size.y));
    }
}
