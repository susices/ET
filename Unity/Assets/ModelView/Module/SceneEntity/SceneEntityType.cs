using System;

namespace ET
{
    [Serializable]
    public enum SceneEntityType
    {
        None,
        Character,
        Interaction,
        Pickable,
        TriggerBox,
        Building,
    }
}