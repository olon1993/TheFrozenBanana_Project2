using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class BreakablePlatformController : RaycastController
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        private Animator _animator;

        [SerializeField] protected LayerMask _breakMask;
        [TagSelector]
        [SerializeField] private string[] _tagFilterArray = new string[] { };
        [SerializeField] protected float _timeToBreak = 1f;
        [SerializeField] protected float _rayLength = 0.25f;
        private bool _isDestroyInvoked = false;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.Log("Animator not found on " + name);
            }
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            UpdateRacastOrigins();
        }

        protected virtual void Update()
        {
            if (_isDestroyInvoked == false)
            {
                for (int i = 0; i < _verticalRayCount; i++)
                {
                    Vector2 rayOrigin = _raycastOrigins.TopLeft;

                    rayOrigin += Vector2.right * (_verticalRaySpacing * i);
                    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, _rayLength, _breakMask);

                    if (_showDebugLog)
                    {
                        Debug.DrawRay(rayOrigin, Vector2.up * _rayLength, Color.red);
                    }

                    if (hit && hit.distance != 0)
                    {
                        foreach (string tag in _tagFilterArray)
                        {
                            if (hit.transform.CompareTag(tag))
                            {
                                if (_showDebugLog)
                                {
                                    Debug.Log("Crumble invoked on " + name);
                                }

                                Invoke(nameof(BreakPlatform), _timeToBreak);
                                _animator.SetTrigger("Break");
                                _isDestroyInvoked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void BreakPlatform()
        {
            IOnDeath onDeath = GetComponent<IOnDeath>();
            if (onDeath != null)
                onDeath.DoThisOnDeath();
            Destroy(gameObject);
        }
    }
}
