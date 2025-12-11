using UnityEngine;

namespace Spelprojekt1
{
    public class ShooterStateMachine : MonoBehaviour
    {
        public enum ShootState
        {
            Idle,
            Charging,
            ChargedRelease
        }

        public ShootState State { get; private set; } = ShootState.Idle;

        private float chargeTimer = 0f;
        private float cooldownTimer = 0f;

        private readonly float chargeTime;
        private readonly float fireCooldown;

        public bool CanStartCharging => cooldownTimer >= fireCooldown;
        public float ChargePercent => Mathf.Clamp01(chargeTimer / chargeTime);

        public ShooterStateMachine(float chargeTime, float cooldown)
        {
            this.chargeTime = chargeTime;
            this.fireCooldown = cooldown;
        }

        public void Tick(bool fire1Held, bool fire1Released, float deltaTime)
        {
            cooldownTimer += deltaTime;

            switch (State)
            {
                case ShootState.Idle:
                    if(fire1Held && CanStartCharging)
                    {
                        //StartCharging();
                    }
                    break;
            }
        }
    }
}
