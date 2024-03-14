using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "EntityTypes/Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("El tipo de enemigo a instanciar.")]
    public string type;

    [Tooltip("La velocidad del enemigo.")]
    [Range(.5f, 3f)]
    public float speed;


    [Tooltip("El rango de detección del enemigo.")]
    public float detectionRange;

    [Tooltip("La velocidad de ataque del enemigo.")]
    public float atkSpeed;
}
