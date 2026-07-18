using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Demo.Logic
{
    public class TEST_TWO : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            Debug.Log("TEST_TWO");
        }
    }
}