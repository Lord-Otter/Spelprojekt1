using UnityEngine;

namespace Spelprojekt1
{
    public class InputAI : InputBase
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            if (target == null)
            {
                SetMoveInput(Vector2.zero);
                return;
            }

            Vector2 direction = (target.position - transform.position).normalized;

            SetMoveInput(direction);
        }
    }
}
