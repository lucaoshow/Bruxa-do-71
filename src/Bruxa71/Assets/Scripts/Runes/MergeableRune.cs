using UnityEngine;
using Root.EditorExtensions.PropertyDrawers;
using System;

namespace Root.Runes
{
    public class MergeableRune : Rune
    {
        [SerializeField] private MergeableRuneTypes runeType;

        [MergeConfigurationProperty]
        [SerializeField] private Rune[] mergeConfiguration = new Rune[Enum.GetValues(typeof(MergeableRuneTypes)).Length];

        public Rune Merge(MergeableRuneTypes otherType)
        {
            return this.mergeConfiguration[(int) otherType];
        }

        public MergeableRuneTypes GetRuneType()
        {
            return this.runeType;
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