using Generated;
using Leopotam.EcsLite;
using UnityEngine;

public sealed class TestOne : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        Debug.Log(EcsRunSystemsIds.TestOne);
    }
}