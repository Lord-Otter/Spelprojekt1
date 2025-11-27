using UnityEngine;
using UnityEngine.UI;

namespace Spelprojekt1
{
    public class InputBase : MonoBehaviour
    {
        public Vector2 moveInput { get; protected set; }

        protected virtual void SetMoveInput(Vector2 value)
        {
            moveInput = value;
        }
    }
}
