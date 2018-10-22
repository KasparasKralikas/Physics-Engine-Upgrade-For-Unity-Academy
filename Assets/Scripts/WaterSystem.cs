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

        

        for (var i = 0; i < entities.flags.Count; i++)
        {
            if (entities.flags[i].HasFlag(EntityFlags.kFlagWater) &&
                entities.flags[i].HasFlag(EntityFlags.kFlagForce))
            {
                var forceComponent = entities.forceComponents[i];
                var moveComponent = entities.moveComponents[i];
                var collisionComponent = entities.collisionComponents[i];
                var position = entities.positions[i];
                float volume = collisionComponent.radius * collisionComponent.radius * Mathf.PI;
                

                if (forceComponent.massInverse > 1e-6f)
                {
                    if (position.y - collisionComponent.radius <= waterLevel)
                    {
                        float displacedVolume = volume;

                        if (waterLevel - position.y < collisionComponent.radius)
                        {

                        }

                        // F = waterDensity * V * -g

                        forceComponent.force += waterDensity * displacedVolume * - gravity;
                        
                    }
                }

                entities.forceComponents[i] = forceComponent;
            }
        }
    }
}
