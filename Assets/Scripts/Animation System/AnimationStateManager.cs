using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class AnimationStateManager : MonoBehaviour, IAnimationManager
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private Animator _animator;
        private AnimationState _currentAnimationState;
        private ILocomotion _locomotion;
        private ICombatant _combatant;

        [SerializeField] List<AnimationState> ImmutableAnimationStates;

        private const string IDLE_RIGHT_NAME = "Idle_right";
        private const string IDLE_LEFT_NAME = "Idle_left";
        private const string WALK_RIGHT_NAME = "Run_right";
        private const string WALK_LEFT_NAME = "Run_left";
        private const string DASH_START_RIGHT_NAME = "Dash_start_right";
        private const string DASH_START_LEFT_NAME = "Dash_start_left";
        private const string DASH_STOP_RIGHT_NAME = "Dash_stop_right";
        private const string DASH_STOP_LEFT_NAME = "Dash_stop_left";
        private const string ATTACK_RIGHT_NAME = "Attack_right";
        private const string ATTACK_LEFT_NAME = "Attack_left";
        private const string JUMP_RIGHT_NAME = "Jump_right";
        private const string JUMP_LEFT_NAME = "Jump_left";
        private const string FALL_RIGHT_NAME = "Fall_right";
        private const string FALL_LEFT_NAME = "Fall_left";
        private const string WALL_SLIDE_LEFT_NAME = "Wall_slide_left";
        private const string WALL_SLIDE_RIGHT_NAME = "Wall_slide_right";

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        private void Awake()
        {
            _animator = transform.Find("Graphics").GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator not found on " + gameObject.name);
            }

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
        }

        // Start is called before the first frame update
        void Start()
        {
            RequestStateChange(AnimationState.IDLE_RIGHT);
        }

        void Update()
        {
            // Attack
            if (_combatant.IsAttacking)
            {
                if (_locomotion.HorizontalLook > 0)
                {
                    RequestStateChange(AnimationState.ATTACK_RIGHT);
                }
                else
                {
                    RequestStateChange(AnimationState.ATTACK_LEFT);
                }
            }
            // Dash
            else if (_locomotion.IsDashCancelled)
            {
                if (GetCurrentAnimationState() == AnimationState.DASH_START_RIGHT)
                {
                    RequestStateChange(AnimationState.DASH_STOP_RIGHT);
                }
                else if (GetCurrentAnimationState() == AnimationState.DASH_START_LEFT)
                {
                    RequestStateChange(AnimationState.DASH_STOP_LEFT);
                }
            }
            else if (Mathf.Abs(_locomotion.Velocity.y) > 0.25f)
            {
                if (_locomotion.HorizontalLook > 0)
                {
                    if (_locomotion.IsRightCollision)
                    {
                        RequestStateChange(AnimationState.WALL_SLIDE_RIGHT);
                    }
                    else if (_locomotion.Velocity.y > 0)
                    {
                        RequestStateChange(AnimationState.JUMP_RIGHT);
                    }
                    else
                    {
                        RequestStateChange(AnimationState.FALL_RIGHT);
                    }
                }
                else
                {
                    if (_locomotion.IsLeftCollision)
                    {
                        RequestStateChange(AnimationState.WALL_SLIDE_LEFT);
                    }
                    else if (_locomotion.Velocity.y > 0)
                    {
                        RequestStateChange(AnimationState.JUMP_LEFT);
                    }
                    else
                    {
                        RequestStateChange(AnimationState.FALL_LEFT);
                    }
                }
            }
            else
            {
                if (_locomotion.HorizontalMovement != 0)
                {
                    if (_locomotion.HorizontalMovement > 0)
                    {
                        if (_locomotion.IsDashing && _locomotion.IsGrounded)
                        {
                            RequestStateChange(AnimationState.DASH_START_RIGHT);
                        }
                        else
                        {
                            RequestStateChange(AnimationState.WALK_RIGHT);
                        }
                    }
                    else
                    {
                        if (_locomotion.IsDashing && _locomotion.IsGrounded)
                        {
                            RequestStateChange(AnimationState.DASH_START_LEFT);
                        }
                        else
                        {
                            RequestStateChange(AnimationState.WALK_LEFT);
                        }
                    }
                }
                else
                {
                    if (_locomotion.HorizontalLook > 0)
                    {
                        RequestStateChange(AnimationState.IDLE_RIGHT);
                    }
                    else
                    {
                        RequestStateChange(AnimationState.IDLE_LEFT);
                    }
                }
            }
        }

        // Call this method externally whenever the animation state must be updated
        public void RequestStateChange(AnimationState newState)
        {
            // Do no further processing if the requested state is already active
            // This is important for RequestStateChange being used in Update or Coroutines
            if (_currentAnimationState == newState)
            {
                return;
            }

            // Do no further processing if the animation cannot be interrupted
            if (CanInterrupt() == false)
            {
                return;
            }

            _currentAnimationState = newState;

            // New animations need to be added to this switch statement
            switch (newState)
            {
                case AnimationState.IDLE_RIGHT:
                    _animator.Play(IDLE_RIGHT_NAME);
                    break;
                case AnimationState.IDLE_LEFT:
                    _animator.Play(IDLE_LEFT_NAME);
                    break;
                case AnimationState.WALK_RIGHT:
                    _animator.Play(WALK_RIGHT_NAME);
                    break;
                case AnimationState.WALK_LEFT:
                    _animator.Play(WALK_LEFT_NAME);
                    break;
                case AnimationState.DASH_START_RIGHT:
                    _animator.Play(DASH_START_RIGHT_NAME);
                    break;
                case AnimationState.DASH_START_LEFT:
                    _animator.Play(DASH_START_LEFT_NAME);
                    break;
                case AnimationState.DASH_STOP_RIGHT:
                    _animator.Play(DASH_STOP_RIGHT_NAME);
                    break;
                case AnimationState.DASH_STOP_LEFT:
                    _animator.Play(DASH_STOP_LEFT_NAME);
                    break;
                case AnimationState.ATTACK_RIGHT:
                    _animator.Play(ATTACK_RIGHT_NAME);
                    break;
                case AnimationState.ATTACK_LEFT:
                    _animator.Play(ATTACK_LEFT_NAME);
                    break;
                case AnimationState.JUMP_LEFT:
                    _animator.Play(JUMP_LEFT_NAME);
                    break;
                case AnimationState.JUMP_RIGHT:
                    _animator.Play(JUMP_RIGHT_NAME);
                    break;
                case AnimationState.FALL_LEFT:
                    _animator.Play(FALL_LEFT_NAME);
                    break;
                case AnimationState.FALL_RIGHT:
                    _animator.Play(FALL_RIGHT_NAME);
                    break;
                case AnimationState.WALL_SLIDE_LEFT:
                    _animator.Play(WALL_SLIDE_LEFT_NAME);
                    break;
                case AnimationState.WALL_SLIDE_RIGHT:
                    _animator.Play(WALL_SLIDE_RIGHT_NAME);
                    break;
            }
        }

        private bool CanInterrupt()
        {
            if (ImmutableAnimationStates.Contains(_currentAnimationState) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                return false;
            }

            return true;
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public AnimationState GetCurrentAnimationState()
        {
            return _currentAnimationState;
        }

    }
}
