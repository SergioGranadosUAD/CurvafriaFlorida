using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "EntityTypes/Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("El tipo de enemigo a instanciar.")]
    public string type;

    [Tooltip("La velocidad del enemigo.")]
    [Range(1f, 5f)]
    public float speed;


    [Tooltip("El rango de detecci�n del enemigo.")]
    public float detectionRange;

    [Tooltip("El rango de ataque.")]
    public float atkRange;

    [Tooltip("La distancia m�nima que el enemigo intentar� tener entre el jugador.")]
    public float distanceRange;

    [Tooltip("La velocidad de ataque del enemigo.")]
    public float atkSpeed;
}
