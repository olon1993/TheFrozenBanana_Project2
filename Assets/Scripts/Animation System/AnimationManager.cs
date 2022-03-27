using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheFrozenBanana
{
    public class AnimationManager : MonoBehaviour, IAnimationManager
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private Animator _animator;
		private Combatant _combatant;
		[SerializeField] private List<IAnimationState> _animationStates;

        private IAnimationState _currentAnimationState;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator not found on " + gameObject.name);
			}

			_combatant = GetComponentInParent<Combatant>();

			if (_combatant == null) {
				Debug.Log("The object '" + gameObject.name + "' has no combatant. only single layer animations will be run.");
			}

			_animationStates = GetComponents<IAnimationState>().ToList();
            
            if(_animationStates.Count == 0)
            {
                Debug.LogError("No Animation States found on " + name);
            }
            else
            {
                _animationStates.Sort();
            }

        }

        private void Start()
        {
            _currentAnimationState = _animationStates[_animationStates.Count - 1];
            _currentAnimationState.OnStateEnter();
            _animator.Play(_currentAnimationState.Animation.name);
        }

        private void LateUpdate()
        {
            EvaluateState();
        }

        private void EvaluateState()
        {
            if(_animationStates.Select(x => x.IsAnimationOverride).Any())
            {
                Debug.Log("HERE");
            }

            foreach (IAnimationState state in _animationStates)
            {
				if (state.ShouldPlay()) 
				{
					if (_combatant != null) 
					{
						if (state.AnimationLayer != _combatant.CurrentWeapon.AnimationLayer) {
							continue;
						}
					}
					CurrentAnimationState = state;
					break;
				}
			}
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public IAnimationState CurrentAnimationState 
        {
            get { return _currentAnimationState; }
            private set
            {
                if (_currentAnimationState != value)
                {
                    if (_currentAnimationState.CanInterrupt == false && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                    {
                        return;
                    }

                    _currentAnimationState.OnStateExit();
                    _currentAnimationState = value;
                    _currentAnimationState.OnStateEnter();
                    _animator.Play(_currentAnimationState.Animation.name);
                    _currentAnimationState.OnStateExecute();
                }
            }
        }
    }
}
