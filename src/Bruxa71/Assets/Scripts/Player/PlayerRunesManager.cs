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
        private int leftIndex;
        private Rune rightSelected;
        private int rightIndex;

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

            if (this.changeLeft.WasPressedThisFrame())
            {
                this.leftIndex = this.UpdateSelectedIndex(this.leftIndex, this.rightIndex);
                this.leftSelected = this.playerData.runes[this.leftIndex];
                // change image accordingly
            }

            if (this.changeRight.WasPressedThisFrame())
            {
                this.rightIndex = this.UpdateSelectedIndex(this.rightIndex, this.leftIndex);
                this.rightSelected = this.playerData.runes[this.rightIndex];
                // change image accordingly
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

        private int UpdateSelectedIndex(int index, int otherIndex)
        {
            index += index + 1 == this.playerData.runes.Count ? -index : 1;
            if (index == otherIndex) { index = (index + 1) % this.playerData.runes.Count; }

            return index;
        }
    }
}
