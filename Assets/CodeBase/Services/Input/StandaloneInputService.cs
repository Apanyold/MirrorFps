using UnityEngine;

namespace CodeBase.Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis => UnityAxis();

        public override bool IsAttackButtonUp() => UnityEngine.Input.GetButtonDown("Fire1");

        public override bool isMenuButtonDown() => UnityEngine.Input.GetKeyDown(KeyCode.Escape);

        private static Vector2 UnityAxis() =>
          new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
    }
}