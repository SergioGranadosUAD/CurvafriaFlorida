using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public GameObject WeaponRoot { get; set; }
    public GameObject ProjectilePrefab { get; set; }
    public Quaternion RotationAngle { get; set; } 
    public string BulletTag {  get; set; }
    public int BulletCount { get; set; }
    void SetWeaponData(WeaponData data, int currentAmmo);
    void Attack();
}
