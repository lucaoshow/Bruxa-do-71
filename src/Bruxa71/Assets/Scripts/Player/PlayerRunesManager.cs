using UnityEngine;
using Root.Runes;
using UnityEngine.InputSystem;

namespace Root.Player
{
    public class PlayerRunesManager : MonoBehaviour
    {
         [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private PlayerData playerData;
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
            this.leftSelected = this.playerData.runes[0];
            foreach (Rune rune in this.playerData.runes) 
            {
                Instantiate(rune, this.transform.position, Quaternion.identity, this.transform);
            }
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

        public void AddStatus(EffectOnPlayer status)
        {
            this.transform.position += (this.GetAimPosition() - this.transform.position).normalized * status.distanceToTravel;
            this.playerData.moveSpeed += status.speedBuff;
        }

        private void ActivateRune(Rune rune) 
        {
            rune.Activate(this, (this.GetAimPosition() - this.transform.position).normalized);
        }

        private Vector3 GetAimPosition() 
        {
            Vector2 aimScreenPos = this.aim.ReadValue<Vector2>();
            Vector3 aimPos = Camera.main.ScreenToWorldPoint(new Vector3(aimScreenPos.x, aimScreenPos.y, 0));
            aimPos.z = 0;
            return aimPos;
        }
    }
}
