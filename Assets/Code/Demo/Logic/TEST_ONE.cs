using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Logic.Game
{
    public class TEST_ONE : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            Debug.Log("TEST_ONE");
        }
    }
}