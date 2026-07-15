using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Configs.Ecs
{
    [CreateAssetMenu(fileName = "EcsConfig", menuName = "Configs/ECS Config", order = 3)]
    public sealed class EcsConfig : SerializedScriptableObject
    {
        // [PropertySpace(SpaceBefore = 15)] [ListDrawerSettings(ShowFoldout = false, ShowItemCount = false, NumberOfItemsPerPage = 100)]
        // public FeatureGroupContainer[] FeatureGroups;
    }
}