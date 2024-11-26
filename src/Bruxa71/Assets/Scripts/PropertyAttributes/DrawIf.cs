using UnityEngine;

namespace Root.PropertyAttributes
{
    public class DrawIf : PropertyAttribute
    {
        public string comparedPropertyName { get; private set; }
        public object comparedValue { get; private set; }

        public DrawIf(string comparedPropertyName, object comparedValue)
        {
            this.comparedPropertyName = comparedPropertyName;
            this.comparedValue = comparedValue;
        }
    }
}