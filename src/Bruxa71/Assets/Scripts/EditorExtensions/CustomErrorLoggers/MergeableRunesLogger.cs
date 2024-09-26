using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Root.Runes;

namespace Root.EditorExtensions.CustomErrorLoggers
{
    public class MergeableRunesLogger
    {
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CheckMergeableRunesInconsistencies()
        {
            List<MergeableRuneTypes> enumVals = new List<MergeableRuneTypes>((MergeableRuneTypes[]) Enum.GetValues(typeof(MergeableRuneTypes)));
            List<GameObject> runesPrefabs = Resources.LoadAll<GameObject>("Prefabs/Runes/Mergeable").ToList();
            List<string> prefabsNames = runesPrefabs.ConvertAll<string>(prefab => prefab.name);
            List<MergeableRune> runesPrefabsScripts = runesPrefabs.ConvertAll(prefab => prefab.GetComponent<MergeableRune>());

            for (int i = 0; i < runesPrefabs.Count; i++ )
            {
                string typeName = prefabsNames[i];
                MergeableRune rune = runesPrefabsScripts[i];

                if (rune.GetRuneEffect() == null)
                {
                    Debug.LogError(typeName + " prefab does not have a RuneEffect assigned, this will cause errors when trying to activate the rune. Fix this issue by dragging a RuneEffect prefab to the correspondent Rune Effect field on the " + typeName + " prefab.");
                }

                foreach (MergeableRuneTypes mergeableRuneType in enumVals)
                {
                    string enumTypeName = mergeableRuneType.ToString();
                    if (!typeName.Equals(enumTypeName) && rune.Merge(mergeableRuneType) == null)
                    {
                        Debug.LogError(typeName + " prefab does not have a resulting Rune when merging with " + enumTypeName + ". Implement it by dragging the result Rune prefab to the correspondent Merge Configuration field on the " + typeName + " prefab.");
                    }
                }
            }

            List<string> noEquivalentPrefab = enumVals.ConvertAll<string>(v => v.ToString()).FindAll(n => !prefabsNames.Contains(n)).ToList();
            foreach (string name in noEquivalentPrefab)
            {
                Debug.LogError("MergeableRune of type " + name + " has not been implemented in a prefab object for usage. Fix this issue by creating a prefab for the type and putting it in the \"Prefabs/Runes/Mergeable\" directory.");
            }

            var prefabsWithSameRuneType = runesPrefabs.GroupBy(prefab => prefab.GetComponent<MergeableRune>().GetRuneType()).Where(group => group.Count() > 1).ToList();

            foreach (var typeAndPrefab in prefabsWithSameRuneType)
            {
                Debug.LogError("Prefabs named " + String.Join(", ", typeAndPrefab.Select(g => g.name)) + " implement the same Mergeable Rune Type " + typeAndPrefab.Key + ". Mergeable Rune Types must not be shared.");
            }
        }
    }
}
