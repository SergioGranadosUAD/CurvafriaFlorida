using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "EntityTypes/Weapons")]
public class WeaponData : ScriptableObject
{
    [Tooltip("El modelo del arma.")]
    public GameObject weapon;

    [Tooltip("Posición del arma respecto a la mano.")]
    public Vector3 gripPosition;

    [Tooltip("Rotación del arma respecto a la mano (Ángulos en Euler).")]
    public Vector3 gripRotation;

    [Tooltip("Posición del cañón del arma (Para instanciar proyectiles).")]
    public Vector3 barrelPosition;

    [Tooltip("El tipo de arma.")]
    public string type;

    [Tooltip("La cadencia de disparo del arma.")]
    [Range(.2f, 10f)]
    public float rateOfFire;

    [Tooltip("El tamaño del cargador del arma.")]
    public int maxAmmo;

    [Tooltip("Ángulo de desviación de la bala (Más/menos).")]
    [Range(0f, 20f)]
    public float spreadAngle;
}
