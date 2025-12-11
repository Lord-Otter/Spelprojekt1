using UnityEngine;

namespace Spelprojekt1
{
    public class PlayerProjectile : ProjectileBehaviour
    {
        [SerializeField] private bool pierce;

        public override void Initialize(float damage, float knockback, float range, bool pierce)
        {
            base.Initialize(damage, knockback, range, pierce);
            this.pierce = pierce;
        }
    }
}


