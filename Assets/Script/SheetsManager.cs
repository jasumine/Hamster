using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class SheetsManager : MonoBehaviour
{
    private string spreadsheetUrl = "AKfycbwkLTqFsY8eFRISCvRRdZxJ6t4rfjTUItVVShY3F0lnL_C0uq-jmsD8EW84lIN6AhPF\r\n";

    public void WriteData(string _playerId, string _playerName, int _score, float _clearTime)
    {
        StartCoroutine(PostData(_playerId, _playerName, _score, _clearTime));
    }

    public void ReadData()
    {
        StartCoroutine(GetData());
    }

    public IEnumerator PostData(string _playerId, string _playerName, int _score, float _clearTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("PlayerId", _playerId);
        form.AddField("PlayerName", _playerName);
        form.AddField("Score", _score.ToString());
        form.AddField("ClearTime", _clearTime.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(spreadsheetUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(spreadsheetUrl + "?action=getRankings"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

}

