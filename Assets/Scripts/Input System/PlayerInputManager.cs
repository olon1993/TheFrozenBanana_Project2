using UnityEngine;

namespace TheFrozenBanana
{
    public class PlayerInputManager : MonoBehaviour, IInputManager
    {
        [SerializeField] protected bool _showDebugLog = false;

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
        private bool _isAttack;
        [SerializeField] private bool _isEnabled;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        void Update()
        {
            if (PauseGameState.Instance !=null && PauseGameState.Instance.GamePaused) { return; }

            if (IsEnabled)
            {
                _horizontal = Input.GetAxisRaw("Horizontal");
                _vertical = Input.GetAxisRaw("Vertical");

                _isJump = Input.GetButtonDown("Jump");
                _isJumpCancelled = Input.GetButtonUp("Jump");

                _isDash = Input.GetButton("Fire3");

                _isAttack = Input.GetButtonDown("Fire1");

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
            else
            {
                _horizontal = 0;
                _vertical = 0;
                _isJump = false;
                _isJumpCancelled = false;
                _isDash = false;
                _isAttack = false;
            }
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public float Horizontal { get { return _horizontal; } }

        public float Vertical { get { return _vertical; } }

        public bool IsJump { get { return _isJump; } }

        public bool IsJumpCancelled { get { return _isJumpCancelled; } }

        public bool IsDash { get { return _isDash; } }

        public bool IsAttack { get { return _isAttack; } }

        public bool IsEnabled 
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
            }
        }
    }
}
