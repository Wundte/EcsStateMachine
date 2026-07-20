using Code.Demo.Runtime.Data.EcsComponents.Cubes;
using Code.Demo.Runtime.Data.EcsComponents.СubesMovement;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Demo.Runtime.Logic.CubeMovement.BlueCubeMovement
{
    public sealed class BlueCubeMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BlueCubeComponent>> _filter = default;

        private readonly EcsPoolInject<TransformComponent> _transformComponentPool = default;
        private readonly EcsPoolInject<MoveComponent> _moveComponentPool = default;
        private readonly EcsPoolInject<MovementBoundsComponent> _movementBoundsComponentPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var transformComponent = ref _transformComponentPool.Value.Get(entity);
                ref var moveComponent = ref _moveComponentPool.Value.Get(entity);
                ref var movementBoundsComponent = ref _movementBoundsComponentPool.Value.Get(entity);

                var position = transformComponent.Transform.position;

                position.x += moveComponent.Direction * moveComponent.Speed * Time.deltaTime;
                
                if (position.x >= movementBoundsComponent.MaxX)
                {
                    position.x = movementBoundsComponent.MaxX;
                    moveComponent.Direction = -1f;
                }
                else if (position.x <= movementBoundsComponent.MinX)
                {
                    position.x = movementBoundsComponent.MinX;
                    moveComponent.Direction = 1f;
                }

                transformComponent.Transform.position = position;
            }
        }
    }
}