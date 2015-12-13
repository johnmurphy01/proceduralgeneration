using UnityEngine;
using System.Collections;
using FPSControl.Data;

public class ScreenCapture : MonoBehaviour {

    public Texture2D screenshot;
    
    // Use this for initialization
	void Start () {
        StartCoroutine(WaitAndCapture());
	}

    IEnumerator WaitAndCapture()
    {
        yield return new WaitForSeconds(2f);
        ScreenShot.Capture("Player Camera", "TestFile001", OnCaptureComplete);
    }

    void OnCaptureComplete()
    {
        Debug.Log("Capture Complete");
        ScreenShot.Load("TestFile001", OnScreenShotLoad);
    }

    void OnScreenShotLoad(Texture2D tex)
    {
        Debug.Log("Load Complete");
        screenshot = tex;
        if (screenshot == null) Debug.LogError("null texture");
    }

    void OnGUI()
    {
        if (!screenshot) return;

        GUI.DrawTexture(new Rect(Screen.width - screenshot.width, 0, screenshot.width, screenshot.height), screenshot);
    }
}
