using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSystem : ISystemInterface
{

    public void Start(World world)
    {
        var entities = world.entities;

        // add randomized velocity to all entities that have positions
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

    }

}
