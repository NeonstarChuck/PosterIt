using UnityEngine;

public class PermissionRedirector : MonoBehaviour
{
    public void OpenAppSettings()
    {
        #if UNITY_ANDROID
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject uri = new AndroidJavaClass("android.net.Uri")
                .CallStatic<AndroidJavaObject>("parse", "package:" + currentActivity.Call<string>("getPackageName"));
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uri);
            currentActivity.Call("startActivity", intent);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open app settings: " + e.Message);
        }
        #else
        Debug.LogWarning("This functionality is only available on Android.");
        #endif
    }
}

