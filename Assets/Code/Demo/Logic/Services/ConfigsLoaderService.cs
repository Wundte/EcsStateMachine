using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.Demo.Logic.Services
{
    public sealed class ConfigsLoaderService : IDisposable
    {
        private const string ConfigLabel = "Configs";
        
        private readonly List<AsyncOperationHandle> _handles = new();
        
        private Dictionary<Type, ScriptableObject> _configs = new();
        
        public async UniTask<Dictionary<Type, ScriptableObject>> Init()
        {
            var handle = Addressables.LoadAssetsAsync<ScriptableObject>(
                    ConfigLabel,
                    config =>
                    {
                        _configs[config.GetType()] = config;
                    });

            _handles.Add(handle);

            await handle.ToUniTask();

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception("Failed to load configs");
            }

            return _configs;
        }
        
        public void Dispose()
        {
            foreach (var handle in _handles)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
        
            _handles.Clear();
        
            _configs?.Clear();
            _configs = null;
        }
    }
}