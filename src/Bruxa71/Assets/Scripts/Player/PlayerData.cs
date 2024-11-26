using System.Collections.Generic;
using Root.Runes;
using UnityEngine;

namespace Root.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public float health;
        public float maxHealth;
        public float moveSpeed;
        public List<Rune> runes; //{ get; private set; } = new List<Rune>();

        public void AddRune(Rune rune)
        {
            this.runes.Add(rune);
        }
    }
}
