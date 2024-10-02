using System.Collections.Generic;
using UnityEngine;
using Root.Runes;

namespace Root.Player
{
    public class PlayerRunesManager : MonoBehaviour
    {
        [SerializeField] private Rune selectedRune;
        public void ActivateSelectedRune(Vector2 playerDirection) 
        {
            this.selectedRune.Activate(this, playerDirection);
        }

        public void AddStatus(EffectOnPlayer status)
        {

        }
    }
}
