using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
public class CheatMenu : EditorWindow
{
    private List<string> weaponNames = new List<string>() { "Melee", "Pistol", "Rifle", "Shotgun" };
    private int selectionIndex = 0;

    private bool godmode = false;
    private bool noTarget = true;
    private bool bottomlessClip = false;

    private bool updated = false;

    [MenuItem("Tools/Cheat Menu")]
    public static void ShowWindow()
    {
        GetWindow<CheatMenu>(false, "Cheat Menu");
    }

    private void OnGUI()
    {
        if (GameManager.Instance.Player == null)
        {
            GUILayout.Label("Player couldn't be found.", EditorStyles.boldLabel);
        }
        else
        {
            SetInspectorValues();
            GUILayout.Label("CHEAT LIST", EditorStyles.boldLabel);
            godmode = GUILayout.Toggle(godmode, "Enable Godmode");
            noTarget = GUILayout.Toggle(noTarget, "Is Targetable");
            bottomlessClip = GUILayout.Toggle(bottomlessClip, "Enable Bottomless Clip");

            if(GUI.changed)
            {
                UpdatePlayer();
            }

            GUILayout.Space(10);

            GUILayout.Label("Give weapon to player", EditorStyles.boldLabel);
            selectionIndex = EditorGUILayout.Popup(selectionIndex, weaponNames.ToArray());
            if (GUILayout.Button("Give weapon"))
            {
                SwitchPlayerWeapon();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Kill all enemies"))
            {
                ClearLevel();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Restart Level"))
            {
                RestartLevel();
            }
        }
    }

    private void SwitchPlayerWeapon()
    {
        WeaponData data;
        switch (selectionIndex)
        {
            case 0:
                data = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Melee");
                break;
            case 1:
                data = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Pistol");
                break;
            case 2:
                data = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Rifle");
                break;
            case 3:
                data = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Shotgun");
                break;
            default:
                data = Resources.Load<WeaponData>("ScriptableObjects/Weapons/Melee");
                break;
        }
        GameManager.Instance.Player.SwitchWeapon(data, data.maxAmmo);
    }

    private void UpdatePlayer()
    {
        GameManager.Instance.Player.GodMode = godmode;
        GameManager.Instance.Player.Targetable = noTarget;
        GameManager.Instance.Player.CurrentWeapon.BottomlessClip = bottomlessClip;
    }

    private void ClearLevel()
    {
        EnemyFactory.Instance.ClearEnemyList();
    }

    private void RestartLevel()
    {
        GameManager.Instance.RestartLevel = true;
    }

    private void SetInspectorValues()
    {
        if(!updated)
        {
            godmode = GameManager.Instance.Player.GodMode;
            noTarget = GameManager.Instance.Player.Targetable;
            bottomlessClip = GameManager.Instance.Player.CurrentWeapon.BottomlessClip;
            updated = true;
        }
    }
}
#endif
