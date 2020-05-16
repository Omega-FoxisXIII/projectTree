﻿using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct AIData : IComponentData
{
    [HideInInspector] public int state;

    public float attackDistancePlayer;
    public float attackDistanceBase;
    public float chaseDistance;

    public int attackDamage;
    public float attackRate;

    [HideInInspector] public float attackWait;

    [HideInInspector] public float attackTime;
    
    [HideInInspector] public int counter;

    [HideInInspector] public bool goToEntity;
    [HideInInspector] public float3 entity;
    [HideInInspector] public float3 storedImpulse;
    public bool stop;
    
}