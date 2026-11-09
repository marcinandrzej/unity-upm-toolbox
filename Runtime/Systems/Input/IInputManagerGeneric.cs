using System;
using UnityEngine.InputSystem;

namespace MA.Systems.Input
{
    public interface IInputManagerGeneric<InputContextT> where InputContextT : struct, Enum
    {
        public PlayerInput PlayerInput { get; }

        public InputContextT ActiveContext { get; }

        public void ClearInputContext();

        public void RegisterInputContext(InputContextT context, object source);

        public void UnregisterInputContext(InputContextT context, object source);
    }
}