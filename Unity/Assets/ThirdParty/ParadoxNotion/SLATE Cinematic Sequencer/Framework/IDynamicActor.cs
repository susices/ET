
using System;
using UnityEngine;

public interface IDynamicActor
{
    GameObject GetActor();
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DynamicActorAttribute:Attribute
{
    
}


