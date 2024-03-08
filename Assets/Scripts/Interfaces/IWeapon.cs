using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void SetWeaponData(GameObject weapon, Vector3 handlePos, Vector3 handleRot, Vector3 barrelPos, string type, float rof, int ammo);
    void Attack();
}
