using System;
using UnityEngine;
using Root.Runes;

namespace Root.PropertyAttributes
{
    public class MergeConfigurationProperty : PropertyAttribute
    {
        public readonly string[] names = Enum.GetNames(typeof(MergeableRuneTypes));
    }
}