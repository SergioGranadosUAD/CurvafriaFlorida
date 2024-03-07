using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "EntityTypes/Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("El tipo de enemigo a instanciar.")]
    public string type;

    //[Tooltip("El controlador de animacion del enemigo a instanciar.")]
    //public RuntimeAnimatorController animatorController;

    //[Tooltip("La vida del enemigo a instanciar.")]
    //[Range(50f, 500f)]
    //public float health;

    [Tooltip("La velocidad del enemigo.")]
    [Range(.5f, 3f)]
    public float speed;


    [Tooltip("El rango de detección del enemigo.")]
    public float detectionRange;

    [Tooltip("La velocidad de ataque del enemigo.")]
    public float atkSpeed;
}
