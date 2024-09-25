using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Root.Runes;

namespace Root.EditorExtensions.CustomErrorLoggers
{
    public class MergeableRunesLogger
    {
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CheckNewTypeAdded()
        {
            List<Type> types = Assembly.GetAssembly(typeof(MergeableRune)).GetTypes().Where(t => t.IsSubclassOf(typeof(MergeableRune))).ToList();
            List<string> enumNames = new List<string>(Enum.GetNames(typeof(MergeableRuneTypes)));

            if (types.Count > enumNames.Count)
            {
                List<Type> outOfEnum = types.FindAll(t => !enumNames.Contains(t.Name)).ToList();
                foreach (Type t in outOfEnum) 
                {
                    Debug.LogError("Enum \"MergeableRuneTypes\" does not have an equivalent value for " + t.Name + ". Add it to the enumerable to fix the issue.");
                }
            }

            if (types.Count < enumNames.Count)
            {
                List<string> typesNames = types.ConvertAll(t => t.Name).ToList();
                List<string> noEquivalentType = enumNames.FindAll(n => !typesNames.Contains(n)).ToList();
                foreach (string name in noEquivalentType)
                {
                    Debug.LogError("MergeableRuneTypes." + name + " does not have an equivalent MergeableRune subclass implementation. Implement a subclass of MergeableRune for the type to fix the issue.");
                }
            }

            List<MergeableRuneTypes> enumVals = new List<MergeableRuneTypes>((MergeableRuneTypes[]) Enum.GetValues(typeof(MergeableRuneTypes)));
            List<MergeableRune> runesPrefabs = Resources.LoadAll<GameObject>("Prefabs/Runes/Mergeable").ToList().ConvertAll(prefab => prefab.GetComponent<MergeableRune>());

            foreach (MergeableRune rune in runesPrefabs)
            {
                string typeName = rune.GetType().Name;
                foreach (MergeableRuneTypes mergeableRuneType in enumVals)
                {
                    if (!typeName.Equals(mergeableRuneType.ToString()) && rune.Merge(mergeableRuneType) == null)
                    {
                        Debug.LogError(typeName + " does not have a resulting Rune when merging with " + mergeableRuneType.ToString() + ". Implement it by dragging the result Rune prefab to the correspondent Merge Configuration field on the " + mergeableRuneType.ToString() + " prefab.");
                    }
                }
            }

            List<string> prefabsTypesNames = runesPrefabs.ConvertAll(t => t.GetType().Name).ToList();
            List<string> noEquivalentPrefab = enumNames.FindAll(n => !prefabsTypesNames.Contains(n)).ToList();

            foreach (string name in noEquivalentPrefab)
            {
                Debug.LogError("MergeableRune of type " + name + " has not been implemented in a prefab object for usage. Fix this issue by creating a prefab for the type and putting it in the \"Prefabs/Runes/Mergeable\" directory.");
            }
        }
    }
}
