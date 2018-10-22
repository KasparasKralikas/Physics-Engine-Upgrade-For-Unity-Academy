using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSystem : ISystemInterface
{

    public void Start(World world)
    {
        var entities = world.entities;

        for (var i = 0; i < entities.flags.Count; i++)
        {
            if (entities.flags[i].HasFlag(EntityFlags.kFlagPosition))
            {
                entities.AddComponent(new Entity(i), EntityFlags.kFlagWater);
            }
        }
        
        
    }

    public void Update(World world, float time = 0, float deltaTime = 0)
    {
        var entities = world.entities;
        var gravity = world.gravity;
        var waterDensity = world.waterDensity;
        var waterLevel = world.waterLevel;

       // F = waterDensity * V * g
    }
}
