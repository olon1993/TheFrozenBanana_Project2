using UnityEngine;

namespace TheFrozenBanana
{
    public class Damage : MonoBehaviour
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\
        [SerializeField] private int _damageAmount;
        [SerializeField] private IWeapon.DamageType _damageType;
        [SerializeField] private float _damageForce = 5f;

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\
        public int DamageAmount
        {
            get { return _damageAmount; }
            set { _damageAmount = value; }
        }

        public IWeapon.DamageType DamageType
        {
            get { return _damageType; }
            set { _damageType = value; }
        }

        public float DamageForce
        {
            get { return _damageForce; }
        }
    }
}
