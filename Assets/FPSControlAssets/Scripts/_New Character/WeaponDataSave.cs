using UnityEngine;
using System.Collections;
using FPSControl;
using FPSControl.Data;

public class WeaponDataSave : MonoBehaviour
{
    public uint slot = 0;

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Save Weapon Data"))
        {
            FPSControlPlayerData.SaveWeaponData(slot);
        }

        GUILayout.EndHorizontal();
    }
        
}
