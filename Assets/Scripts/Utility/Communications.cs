using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

[System.Serializable]
public class Communications<T> : MonoBehaviour
{
    private Coroutine routine;
    public void PostForm(string endpoint, WWWForm form, System.Action<T> callback)
    {
        routine = StartCoroutine(Communicate(Path.Combine(Globals.BASE_URL, endpoint), form, callback));   
    }

    IEnumerator Communicate(string url, WWWForm form, System.Action<T> _callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();
            while (!www.isDone)
                yield return null;

            if (www.isNetworkError || www.isHttpError)
            { 
                T error = default;
                _callback(error);
            }
            else
            {
                T data = JsonUtility.FromJson<T>(www.downloadHandler.text);
                _callback(data);
            }
        }
    }

    public void Terminate()
    {
        if (routine != null) { 
            StopCoroutine(routine);
        }
    }

} 
