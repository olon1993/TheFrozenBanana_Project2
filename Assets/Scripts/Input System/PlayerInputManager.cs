using UnityEngine;

namespace TheFrozenBanana
{
    public class PlayerInputManager : Actor, IInputManager
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Movement
        public bool IsMovementEnabled = true;
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

        // Update is called once per frame
        void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");

            _isJump = Input.GetButtonDown("Jump");
            _isJumpCancelled = Input.GetButtonUp("Jump");

            _isDash = Input.GetButton("Fire3");
            _isDashCancelled = Input.GetButtonUp("Fire3");

            _isAttack = Input.GetButtonDown("Fire1");

            CalculateAnimationState();

            if (_showDebugLog)
            {
                Debug.Log("Horizontal: " + _horizontal);
                Debug.Log("Vertical: " + _vertical);
                Debug.Log("IsJumping: " + _isJump);
                Debug.Log("IsJumpCancelled: " + _isJumpCancelled);
                Debug.Log("IsDashing: " + _isDash);
                Debug.Log("IsAttacking: " + _isAttack);
            }
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
