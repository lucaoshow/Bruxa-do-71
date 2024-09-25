using UnityEngine;
using Root.EditorExtensions.PropertyDrawers;
using System;

namespace Root.Runes
{
    public abstract class MergeableRune : Rune
    {
        [MergeConfigurationProperty]
        [SerializeField] private Rune[] mergeConfiguration;

        public Rune Merge(MergeableRuneTypes runeType)
        {
            return this.mergeConfiguration[(int) runeType];
        }

        private void OnValidate()
        {
            int runes = Enum.GetValues(typeof(MergeableRuneTypes)).Length;
            if (this.mergeConfiguration.Length != runes) 
            {
                Rune[] mergeConfig = this.mergeConfiguration;
                this.mergeConfiguration = new Rune[runes];
                for (int i = 0; i < runes; i++)
                {
                    if (i > mergeConfig.Length - 1) { break; }
                    this.mergeConfiguration[i] = mergeConfig[i];
                }
            }
        }
    }
}