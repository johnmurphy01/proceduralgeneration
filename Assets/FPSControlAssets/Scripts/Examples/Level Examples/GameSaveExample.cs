using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FPSControl;
using FPSControl.Data;

public class GameSaveExample : MonoBehaviour
{
    public static GameSaveExample Instance{ get { return GameObject.Find("[MANAGER]").GetComponent<GameSaveExample>(); } }

    static bool _startup = true;

    static FPSControlPlayerSaveData[] _allSaves = new FPSControlPlayerSaveData[0]{};
    static Dictionary<string, Texture2D> _saveTextures = new Dictionary<string,Texture2D>();
    static int _numTexturesLoaded = 0;
    static bool _loading = true;
    public static FPSControlPlayerSaveData currentSaveData;
    public static uint Slot { get; private set; }

    static void OnTextureLoadComplete(Texture2D texture)
    {
        Debug.Log("Successfully loaded: " + texture.name);
        _saveTextures.Add(texture.name, texture);
        _numTexturesLoaded++;
    }

    IEnumerator WaitForLoadedTextures()
    {
        while (_numTexturesLoaded < _allSaves.Length)
        {
            yield return 0;
            _loading = true;
        }
        _loading = false;
    }

    void Start()
    {
        if (_startup)
        {
            // this is where your data lives
            Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);

            _loading = true;
            //Load all saves...
            _allSaves = FPSControlPlayerData.GetAllSaves();

            //Start Loading all the textures
            foreach (FPSControlPlayerSaveData saveData in _allSaves)
                ScreenShot.Load(saveData.screenshotID, OnTextureLoadComplete);

            //Start a coroutine to track progress
            StartCoroutine(WaitForLoadedTextures());
        }
        else
        {
            // If we're initializing again it's because we're on a new map.
            string currentLevel = Application.loadedLevelName;
            currentSaveData.currentLevelName = currentLevel;
            currentSaveData.spawnPoint = 0;

            // Take a screen shot and autosave. 
            StartCoroutine(TakeScreenShotAndSave(1F));
        }
    }

    IEnumerator TakeScreenShotAndSave(float yieldTime)
    {
        yield return new WaitForSeconds(yieldTime);
        // Note: "Player Camera" is the string ID given in the player camera's prefab on its ScreenShotComponent 
        ScreenShot.Capture("Player Camera", currentSaveData, null); // No need to listen to the capture
        Debug.Log("current save screen shot: " + currentSaveData.screenshotID);
        FPSControlPlayerData.SavePlayerData(currentSaveData, Slot, null);
    }

    void LoadGame(uint slot)
    {
        // first run toggle
        _startup = false;

        Slot = slot;

        // Obtain the save information
        currentSaveData = FPSControlPlayerData.LoadPlayerSaveData(slot);
        
        // weapon data will be loaded from the temporary cache on player spawn
        // so knowing this, what we can do is move over our saved data to a temp and let it all work automatically...

        // load the data
        FPSControlPlayerWeaponManagerSaveData weaponSave = FPSControlPlayerData.LoadWeaponData(Slot);

        if (weaponSave != null) // if we have saved weapons data...
        {
            FPSControlPlayerData.SaveTempWeaponData(weaponSave); // save it to temp cache
            Debug.Log("active weapon: " + weaponSave.activeWeaponName);
        }
        // load the last saved level
        Application.LoadLevel(currentSaveData.currentLevelName);
    }

    void NewGame()
    {
        // first run toggle
        _startup = false;

        Slot = (uint)_allSaves.Length;
        // create a new save data object and save it 
        currentSaveData = new FPSControlPlayerSaveData("SaveTestA", (int) Slot, 100F); //default scene, spawn point, and health
        // load first scene
        Application.LoadLevel("SaveTestA");
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Clear Save Data"))
        {
            PersistentData.DeleteData(PersistentData.NS_PLAYER);
            PersistentData.DeleteData(PersistentData.NS_WEAPONS);
        }
        GUILayout.EndHorizontal();
        
        if (!_startup) return;

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300),GUI.skin.box);

        if(_loading)
        {
            GUILayout.BeginHorizontal();
            
            GUILayout.FlexibleSpace();

            GUILayout.Label("...Loading...");

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            return;
        }

        if (GUILayout.Button("Start New Game"))
            NewGame();

        if (_allSaves.Length < 1)
        {
            GUILayout.EndArea();
            return;
        }
        else
        {
            for (int i = 0; i < _allSaves.Length; i++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Box(_saveTextures[_allSaves[i].screenshotID], GUILayout.MaxWidth(192), GUILayout.MaxHeight(108));
                if (GUILayout.Button("Save " + i))
                    LoadGame((uint)i);

                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndArea();
    }

}
