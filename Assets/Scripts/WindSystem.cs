﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSystem : ISystemInterface
{

    public void Start(World world)
    {
        var entities = world.entities;

        for (var i = 0; i < entities.flags.Count; i++)
        {
            if (entities.flags[i].HasFlag(EntityFlags.kFlagPosition))
            {
                entities.AddComponent(new Entity(i), EntityFlags.kFlagWind);
            }
        }
    }

    public void Update(World world, float time = 0, float deltaTime = 0)
    {
        var entities = world.entities;
        var gravity = world.gravity;
        var windVelocity = world.windVelocity;
        var windResistanceCoeficient = world.windResistanceCoeficient;

        for (var i = 0; i < entities.flags.Count; i++)
        {
            if (entities.flags[i].HasFlag(EntityFlags.kFlagWind) &&
                entities.flags[i].HasFlag(EntityFlags.kFlagForce))
            {
                var forceComponent = entities.forceComponents[i];
                var moveComponent = entities.moveComponents[i];
                var collisionComponent = entities.collisionComponents[i];
                var radius = collisionComponent.radius;

                // F = c * (vWind - vObject)
                // c = k * R

                if (forceComponent.massInverse > 1e-6f)
                {
                    float objectWindResistanceConstant = windResistanceCoeficient * radius;
                    forceComponent.force += objectWindResistanceConstant * (windVelocity - moveComponent.velocity);
                }

                entities.forceComponents[i] = forceComponent;
            }
        }
    }

}
