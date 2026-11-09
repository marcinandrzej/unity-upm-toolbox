using System;

namespace MA.Systems.Input
{
    [Serializable]
    public struct InputContextData
    {
        public string actionMapNameOrId;
        public int priority;
    }
}