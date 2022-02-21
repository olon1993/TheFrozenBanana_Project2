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
    }
}
