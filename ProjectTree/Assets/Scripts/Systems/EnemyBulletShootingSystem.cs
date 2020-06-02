﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class EnemyBulletShootingSystem : ComponentSystem
{
    private static float3 Direction(float3 v1, float3 v2)
    {
        var direction = v2 - v1;
        var magnitude = math.sqrt(math.pow(direction.x, 2) + math.pow(direction.y, 2) + math.pow(direction.z, 2));
        return direction / magnitude;
    }

    protected override void OnUpdate()
    {
        var buffers = GetBufferFromEntity<EnemyPosition>();
        Entities.ForEach((Entity entity, ref AIData aiData, ref BulletPrefabComponent bullet, ref Translation position,
            ref Rotation rotation) =>
        {
            if (aiData.shot)
            {
                Entity bulletEntity = EntityManager.Instantiate(bullet.prefab);
                float3 enemyPos;
                if (aiData.entity != null)
                {
                    enemyPos = EntityManager.GetComponentData<Translation>(aiData.entity).Value;
                    enemyPos.y += 1f;
                }
                else
                    enemyPos = buffers[entity][aiData.counter].position;

                var direction = Direction(position.Value, enemyPos);

                EntityManager.SetComponentData(bulletEntity, new Translation {Value = position.Value});

                var movementData = EntityManager.GetComponentData<MovementData>(bulletEntity);

                movementData.directionX = direction.x;
                movementData.directionY = direction.y;
                movementData.directionZ = direction.z;

                EntityManager.SetComponentData(bulletEntity, movementData);
                aiData.shot = false;
            }
        });
    }
}