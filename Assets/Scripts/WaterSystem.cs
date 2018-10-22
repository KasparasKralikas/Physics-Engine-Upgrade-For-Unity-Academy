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

    private void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    public void Update(World world, float time = 0, float deltaTime = 0)
    {
        var entities = world.entities;
        var gravity = world.gravity;
        var waterDensity = world.waterDensity;
        var waterLevel = world.waterLevel;
        
        // draw the water level - not an effective solution
        DrawLine(new Vector3(world.worldBounds.xMin, waterLevel), new Vector3(world.worldBounds.xMax, waterLevel), Color.blue, deltaTime * 2f);

        for (var i = 0; i < entities.flags.Count; i++)
        {
            if (entities.flags[i].HasFlag(EntityFlags.kFlagWater) &&
                entities.flags[i].HasFlag(EntityFlags.kFlagForce))
            {
                var forceComponent = entities.forceComponents[i];
                var moveComponent = entities.moveComponents[i];
                var collisionComponent = entities.collisionComponents[i];
                var position = entities.positions[i];
                float radius = collisionComponent.radius;
                float volume = radius * radius * Mathf.PI;
                
                

                if (forceComponent.massInverse > 1e-6f)
                {
                    if (position.y - radius <= waterLevel)
                    {
                        float displacedVolume = volume;

                        // if body is not fully submerged, calculate the submerged volume

                        if (Mathf.Abs(waterLevel - position.y) < radius)
                        {
                            float depth = waterLevel - position.y + radius;
                            displacedVolume = (radius * radius / Mathf.Cos((radius - depth) / radius)) - (radius - depth) * Mathf.Sqrt(2 * radius * depth - depth * depth); 
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
