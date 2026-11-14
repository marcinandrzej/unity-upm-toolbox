using MA.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MA.Systems.Settings
{
    public abstract class SettingsManagerGeneric<SettingsDataT> : MonoBehaviour, ISettingsManagerGeneric<SettingsDataT> where SettingsDataT : new()
    {
        [SerializeField] private string settingsFileName = "Settings";
        [SerializeField] private SerializerBase serializer;

        private Dictionary<Type, SettingsSubmanagerGenericBase<SettingsDataT>> _typeToSubmanagerDict = new Dictionary<Type, SettingsSubmanagerGenericBase<SettingsDataT>>();

        public void Initialize() 
        {
            RegisterSubmanagers();

            foreach (var submanager in _typeToSubmanagerDict.Values)
                submanager.InitializeData();
        }

        public void Apply() 
        {
            foreach (var submanager in _typeToSubmanagerDict.Values)
                submanager.Apply();
        }

        public bool TryGetSubmanager<SubmanagerT>(out SubmanagerT submanager) where SubmanagerT : SettingsSubmanagerGenericBase<SettingsDataT> 
        {
            if (_typeToSubmanagerDict.TryGetValue(typeof(SubmanagerT), out SettingsSubmanagerGenericBase<SettingsDataT> value))
            {
                submanager = (SubmanagerT)value;

                return true;
            }
            else
            {
                submanager = null;

                return false;
            }
        }

        public ESerializationOperationResult LoadSettings()
        {
            (SettingsDataT data, ESerializationOperationResult result) operation = serializer.DeserializeDataFromFile<SettingsDataT>(Application.persistentDataPath, settingsFileName);

            if (operation.result.HasFlag(ESerializationOperationResult.Success))
            {
                SettingsDataT settingsData = operation.data;
                PropagateData(settingsData);
            }

            return operation.result;
        }

        public async Awaitable<ESerializationOperationResult> LoadSettingsAsync()
        {
            (SettingsDataT data, ESerializationOperationResult result) operation = await serializer.DeserializeDataFromFileAsync<SettingsDataT>(Application.persistentDataPath, settingsFileName);

            if (operation.result.HasFlag(ESerializationOperationResult.Success))
            {
                SettingsDataT settingsData = operation.data;
                PropagateData(settingsData);
            }

            return operation.result;
        }

        public ESerializationOperationResult SaveSettings()
        {
            SettingsDataT settingsData = CollectData();

            ESerializationOperationResult result = serializer.SerializeDataToFile<SettingsDataT>(Application.persistentDataPath, settingsFileName, settingsData);

            if (result.HasFlag(ESerializationOperationResult.Success))
                PropagateData(settingsData);

            return result;
        }

        public async Awaitable<ESerializationOperationResult> SaveSettingsAsync()
        {
            SettingsDataT settingsData = CollectData();

            ESerializationOperationResult result = await serializer.SerializeDataToFileAsync<SettingsDataT>(Application.persistentDataPath, settingsFileName, settingsData);

            if (result.HasFlag(ESerializationOperationResult.Success))
                PropagateData(settingsData);

            return result;
        }

        protected abstract void RegisterSubmanagers();

        protected bool TryRegisterSubmanager<SubmanagerT>(SubmanagerT submanager) where SubmanagerT : SettingsSubmanagerGenericBase<SettingsDataT>
        {
            return _typeToSubmanagerDict.TryAdd(typeof(SubmanagerT), submanager);
        }

        private void PropagateData(SettingsDataT settingsData) 
        {
                foreach (var submanager in _typeToSubmanagerDict.Values)
                    submanager.LoadFrom(settingsData);
        }

        private SettingsDataT CollectData()
        {
            SettingsDataT settingsData = new SettingsDataT();

            foreach (var submanager in _typeToSubmanagerDict.Values)
                submanager.SaveTo(settingsData);

            return settingsData;
        }
    }
}
