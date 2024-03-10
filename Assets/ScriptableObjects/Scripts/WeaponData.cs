using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "EntityTypes/Weapons")]
public class WeaponData : ScriptableObject
{
    [Tooltip("El modelo del arma.")]
    public GameObject weapon;

    [Tooltip("Posici�n del arma respecto a la mano.")]
    public Vector3 gripPosition;

    [Tooltip("Rotaci�n del arma respecto a la mano (�ngulos en Euler).")]
    public Vector3 gripRotation;

    [Tooltip("Posici�n del ca��n del arma (Para instanciar proyectiles).")]
    public Vector3 barrelPosition;

    [Tooltip("El tipo de arma.")]
    public string type;

    [Tooltip("La cadencia de disparo del arma.")]
    [Range(.2f, 10f)]
    public float rateOfFire;

    [Tooltip("El tama�o del cargador del arma.")]
    public int maxAmmo;

    [Tooltip("�ngulo de desviaci�n de la bala (M�s/menos).")]
    [Range(0f, 20f)]
    public float spreadAngle;
}
