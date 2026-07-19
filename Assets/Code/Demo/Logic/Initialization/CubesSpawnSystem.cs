using Code.Demo.Data.EcsComponents.Cubes;
using Code.Demo.Data.EcsComponents.СubesMovement;
using Code.Demo.Data.UnityComponents.Cubes;
using Code.Logic.Ecs.Interfaces;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Demo.Logic.Initialization
{
    public sealed class CubesSpawnSystem : IEcsStateChangeSystem
    {
        private readonly EcsWorldInject _ecsWorld = default;
        
        private readonly EcsPoolInject<TransformComponent> _transformComponentPool = default;
        private readonly EcsPoolInject<MoveComponent> _moveComponentPool = default;
        private readonly EcsPoolInject<MovementBoundsComponent> _movementBoundsComponentPool = default;
        private readonly EcsPoolInject<WhiteCubeComponent> _whiteCubeComponentPool = default;
        private readonly EcsPoolInject<BlueCubeComponent> _blueCubeComponentPool = default;
        private readonly EcsPoolInject<GreenCubeComponent> _greenCubeComponentPool = default;
        
        public void Run()
        {
            Spawn().Forget();
        }

        private async UniTaskVoid Spawn()
        {
            var handle = Addressables.LoadAssetsAsync<GameObject>("DemoPrefabs", null);

            var prefabs = await handle.Task;

            if (prefabs == null || prefabs.Count == 0)
            {
                Addressables.Release(handle);
                
                return;
            }

            for (var i = 0; i < 6; i++)
            {
                var prefab = prefabs[i % prefabs.Count];
                var instance = Object.Instantiate(prefab, new Vector3(-3, i * 1.5f, 0), Quaternion.identity);
                
                CreateEntity(instance);
            }

            Addressables.Release(handle);
        }
        
        private void CreateEntity(GameObject instance)
        {
            var entity = _ecsWorld.Value.NewEntity();

            ref var transformComponent = ref _transformComponentPool.Value.Add(entity);
            transformComponent.Transform = instance.transform;

            if (instance.TryGetComponent<WhiteCube>(out _))
            {
                _whiteCubeComponentPool.Value.Add(entity);
            }
            else if (instance.TryGetComponent<BlueCube>(out _))
            {
                _blueCubeComponentPool.Value.Add(entity);
            }
            else if (instance.TryGetComponent<GreenCube>(out _))
            {
                _greenCubeComponentPool.Value.Add(entity);
            }
            
            ref var moveComponent = ref _moveComponentPool.Value.Add(entity);
            moveComponent.Direction = 1f;
            moveComponent.Speed = 2f;
            
            ref var movementBounds = ref _movementBoundsComponentPool.Value.Add(entity);
            movementBounds.MinX = instance.transform.position.x;
            movementBounds.MaxX = -instance.transform.position.x;
        }
    }
}
