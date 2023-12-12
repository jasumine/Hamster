using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginManager : MonoBehaviour
{
#if UNITY_ANDROID
    AndroidJavaObject m_AndroidJavaObject;
    AndroidJavaObject m_ActivityInstance;
#endif
    int nNavitiveData;

    int GetInt()
    {
        int nResult = -1;
#if UNITY_STANDALONE_WIN
        nResult = WinfromPlugin.Plugin.GetInt();
#elif UNITY_ANDROID
        if (m_AndroidJavaObject != null)
            nResult = m_AndroidJavaObject.Call<int>("GetInt");
#endif
        return nResult;
    }

    void ShowToastMsg(string msg, int time)
    {
#if UNITY_STANDALONE_WIN

#elif UNITY_ANDROID
        if (m_AndroidJavaObject != null)
        {
            m_AndroidJavaObject.Call("ShowToastMsg", m_ActivityInstance, msg, time);
        }
        else
        {
            Debug.LogError("PluginManager.AndroidJavaObejct is null - ShowToastMsg!");
        }
#else
#endif
    }

    void ShowExitPopup(string msg, string tiltle)
    {
#if UNITY_STANDALONE_WIN
        WinfromPlugin.Plugin.MessageMsg(msg, tiltle, () => { Application.Quit(); });
#elif UNITY_ANDROID
        if (m_AndroidJavaObject != null)
        {
            m_AndroidJavaObject.Call("ShowDialogExit", m_ActivityInstance, msg, tiltle);
        }
        else
        {
            Debug.LogError("PluginManager.AndroidJavaObejct is null - ShowToastMsg!");
        }
#else

#endif
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE_WIN

#elif UNITY_ANDROID
        using (AndroidJavaObject unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            m_ActivityInstance = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            //m_ActivityInstance = uintyPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        m_AndroidJavaObject = new AndroidJavaObject("com.sbsgame.androidplugin.Plugin");
        if (m_AndroidJavaObject != null)
            Debug.LogWarning("PluginManager.AndroidJavaObejct:" + m_AndroidJavaObject);
        else
            Debug.LogError("PluginManager.AndroidJavaObejct is null!");
#else
#endif
    }

    void OnGUI()
    {
        nNavitiveData = GetInt();
#if UNITY_STANDALONE_WIN
        GUI.Box(new Rect(0, 0, 200, 20), "GetInt:" + nNavitiveData);
        if (GUI.Button(new Rect(0, 20, 200, 20), "MessageBox"))
        {
            WinfromPlugin.Plugin.MessageMsg("test!!", "test", () => { Application.Quit(); });
        }
#elif UNITY_ANDROID
        if (m_AndroidJavaObject != null)
        {
            GUI.Box(new Rect(0, 0, 200, 20), "GetInt:" + nNavitiveData);
            if(GUI.Button(new Rect(0, 20, 200, 20), "ShowToast:" + nNavitiveData))
            {
                ShowToastMsg("gdtc 2023", 1);
            }
            if (GUI.Button(new Rect(0, 40, 200, 20), "ShowToast:" + nNavitiveData))
            {
                ShowExitPopup("종료", "종료하시겠습니까?");
            }
        }
        else
        {
            GUI.Box(new Rect(0, 0, 200, 20), "Plugin Load Failed.....");
            GUI.Box(new Rect(0, 0, 200, 20), "Plugin Load Failed.....");
        }
#else
        GUI.Box(new Rect(0, 0, 200, 20), "Plugin Not Support!!!");
        GUI.Box(new Rect(0, 0, 200, 20), "Plugin Not Support!!!");
#endif
    }
}
