using UnityEngine;
using System.Collections;
using FPSControl;
using FPSControl.Controls;

public class FPSControlOuyaController : MonoBehaviour
#if UNITY_ANDROID
    , IOuyaAxis, IOuyaButton
{    
    
    OuyaSDK.OuyaPlayer player = OuyaSDK.OuyaPlayer.player1;

    void Awake()
    {
        if(FPSControlSystemInfo.GetPlatform() == FPSControlPlatform.Ouya)   
            FPSControlInput.OnMapLoad += OnMapLoadHandler;
    }

    void OnDestroy()
    {
        if (FPSControlSystemInfo.GetPlatform() == FPSControlPlatform.Ouya) 
            FPSControlInput.OnMapLoad -= OnMapLoadHandler;
    }

    void OnMapLoadHandler(ControlMap map)
    {
        if (FPSControlSystemInfo.GetPlatform() != FPSControlPlatform.Ouya) return;
        //if (map is OuyaControlMap) { /*proceed...*/ }
        //else { Debug.LogWarning("NOT OUYA MAP!"); return; }
        
        OuyaControlMap ouyaMap = (OuyaControlMap) map;

        ouyaMap.look.Initialize(this, this);
        ouyaMap.movement.Initialize(this, this);

        ouyaMap.crouch.Initialize(this);
        ouyaMap.defend.Initialize(this);
        ouyaMap.fire.Initialize(this);
        ouyaMap.interact.Initialize(this);
        ouyaMap.jump.Initialize(this);
        ouyaMap.reload.Initialize(this);
        ouyaMap.run.Initialize(this);
        ouyaMap.scope.Initialize(this);
        ouyaMap.weapon1.Initialize(this);
        ouyaMap.weapon2.Initialize(this);
        ouyaMap.weapon3.Initialize(this);
        ouyaMap.weapon4.Initialize(this);
        ouyaMap.weaponToggle.Initialize(this);
    }

    public float GetAxis(string axis)
    {
        return OuyaInputManager.GetAxis(axis, player);
    }

    public bool GetButtonDown(string btn)
    {
        return OuyaInputManager.GetButtonDown(btn, player);
    }

    public bool GetButton(string btn)
    {
        return OuyaInputManager.GetButton(btn, player);
    }
#else
{
    #endif
}
