using UnityEngine;

namespace TheFrozenBanana
{
    public class ProjectileWeapon : MonoBehaviour, IWeapon
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] private Transform _pointOfOrigin;
        [SerializeField] Transform PointOfTargetting;
        [SerializeField] Projectile Projectile;
        [SerializeField] private bool _isLimitedAmmo;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private int _currentAmmo;
        [SerializeField] private IWeapon.AmmoType _ammoType;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\
        public void Attack()
        {
            if (IsLimitedAmmo)
            {
                if (CurrentAmmo <= 0)
                {
                    Debug.Log(gameObject.name + " could not attack because they are out of ammo!");
                    return;
                }

                if (MaxAmmo > 0)
                {
                    CurrentAmmo -= 1;
                }
            }

            // Instantiate the weapon gameObject
            Instantiate(Projectile, _pointOfOrigin.position, PointOfTargetting.rotation);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
            Gizmos.DrawRay(_pointOfOrigin.position, direction);
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public bool IsLimitedAmmo
        {
            get { return _isLimitedAmmo; }
            set { _isLimitedAmmo = value; }
        }

        public int MaxAmmo
        {
            get { return _maxAmmo; }
            set { _maxAmmo = value; }
        }

        public int CurrentAmmo
        {
            get { return _currentAmmo; }
            set { _currentAmmo = value; }
        }

        public IWeapon.AmmoType AmmoTypeDefinition
        {
            get { return _ammoType; }
            set { _ammoType = value; }
        }

        public Transform PointOfOrigin
        {
            get { return _pointOfOrigin; }
            set { _pointOfOrigin = value; }
        }

        public float AttackActionTime { get; }
    }
}
