using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Root.EditorExtensions.PropertyDrawers
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

    [CustomPropertyDrawer(typeof(DrawIf))]
    public class DrawIfPropertyDrawer : PropertyDrawer
    {
        DrawIf drawIf;
        SerializedProperty comparedField;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!this.DrawProperty(property)) {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
            
            if(property.propertyType == SerializedPropertyType.Generic)
            {
                float totalHeight = 0f;

                IEnumerator children = property.GetEnumerator();

                while (children.MoveNext())
                {
                    SerializedProperty child = children.Current as SerializedProperty;
                  
                    GUIContent childLabel = new GUIContent(child.displayName);

                    totalHeight += EditorGUI.GetPropertyHeight(child, childLabel) + EditorGUIUtility.standardVerticalSpacing;

                    if (child.type.StartsWith("Vector") && System.Int32.TryParse(child.type.Substring(child.type.Length - 1), out int vectorComponents)) 
                    {
                        for (int i = 0; i < vectorComponents; i++)
                        {
                            children.MoveNext();
                        }
                    }           
                }

                totalHeight -= EditorGUIUtility.standardVerticalSpacing;

                return totalHeight;
            }

            return base.GetPropertyHeight(property, label);
        }

        private bool DrawProperty(SerializedProperty property)
        {
            this.drawIf = attribute as DrawIf;

            comparedField = property.serializedObject.FindProperty(drawIf.comparedPropertyName);
            switch (comparedField.type)
            {
                case "bool":
                    return comparedField.boolValue.Equals(drawIf.comparedValue);
                case "Enum":
                    return comparedField.enumValueIndex.Equals((int)drawIf.comparedValue);
                default:
                    return true;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (this.DrawProperty(property))
            {
                if(property.propertyType == SerializedPropertyType.Generic)
                {
                    IEnumerator children = property.GetEnumerator();

                    Rect offsetPosition = position;

                    while (children.MoveNext())
                    {
                        SerializedProperty child = children.Current as SerializedProperty;
                    
                        GUIContent childLabel = new GUIContent(child.displayName);

                        float childHeight = EditorGUI.GetPropertyHeight(child, childLabel);
                        offsetPosition.height = childHeight;

                        EditorGUI.PropertyField(offsetPosition, child, childLabel);
                    
                        offsetPosition.y += childHeight + EditorGUIUtility.standardVerticalSpacing;
                        if (child.type.StartsWith("Vector") && System.Int32.TryParse(child.type.Substring(child.type.Length - 1), out int vectorComponents)) 
                        {
                            for (int i = 0; i < vectorComponents; i++)
                            {
                                children.MoveNext();
                            }
                        }
                    }
                }
                else
                {
                    EditorGUI.PropertyField(position, property, label);
                }
            }
        }

    }
}
