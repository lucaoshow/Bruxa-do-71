using System.Collections.Generic;
using UnityEngine;
using Root.Runes;
using UnityEngine.InputSystem;

namespace Root.Player
{
    public class PlayerRunesManager : MonoBehaviour
    {
        [SerializeField] private List<Rune> runes;
        [SerializeField] private InputActionAsset inputActions;
        private InputAction shootLeft;
        private InputAction shootRight;
        private InputAction changeLeft;
        private InputAction changeRight;
        private InputAction aim;
        private Rune leftSelected;
        private Rune rightSelected;

        private void Awake()
        {
            InputActionMap combatMap = this.inputActions.FindActionMap("Combat");
            this.shootLeft = combatMap.FindAction("shootLeft");
            this.shootRight = combatMap.FindAction("shootRight");
            this.changeLeft = combatMap.FindAction("changeLeft");
            this.changeRight = combatMap.FindAction("changeRight");
            this.aim = combatMap.FindAction("aim");

            this.shootLeft.Enable();
            this.shootRight.Enable();
            this.changeLeft.Enable();
            this.changeRight.Enable();
            this.aim.Enable();
            this.leftSelected = this.runes[0];
        }

        private void Update()
        {
            if (this.shootLeft.WasPressedThisFrame())
            {
                this.ActivateRune(this.leftSelected);
            }

            if (this.shootRight.WasPressedThisFrame())
            {
                this.ActivateRune(this.rightSelected);
            }
        }

        private void ActivateRune(Rune rune) 
        {
            Vector2 aimScreenPos = this.aim.ReadValue<Vector2>();
            Vector3 aimPos = Camera.main.ScreenToWorldPoint(new Vector3(aimScreenPos.x, aimScreenPos.y, 0));
            aimPos.z = 0;
            rune.Activate(this, (new Vector3(aimPos.x, aimPos.y, 0) - this.transform.position).normalized);
        }

        public void AddStatus(EffectOnPlayer status)
        {

        }
    }
}
