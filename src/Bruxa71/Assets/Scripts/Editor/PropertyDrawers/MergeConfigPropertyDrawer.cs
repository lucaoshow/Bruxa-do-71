using UnityEditor;
using UnityEngine;
using UnityEditor.Rendering;
using Root.Runes;
using Root.PropertyAttributes;

namespace Root.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(MergeConfigurationProperty))]
    public class MergeConfigurationDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            try
            {
                int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
                string name = ((MergeConfigurationProperty)attribute).names[pos];
                if (!name.Equals(property.serializedObject.FindProperty("runeType").GetEnumName<MergeableRuneTypes>()))
                {
                    EditorGUI.ObjectField(rect, property, new GUIContent(((MergeConfigurationProperty)attribute).names[pos]));
                }
                else
                {
                    EditorGUI.LabelField(rect, "Reserved for " + name + " (can't merge equal runes)");
                }
            }
            catch
            {
                EditorGUI.ObjectField(rect, property, label);
            }
        }
    }

}
