using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 
 


public class PasswordPanelManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DownloadPasswordIcons());
    }

    IEnumerator DownloadPasswordIcons()
    { 
        string url = System.IO.Path.Combine(Globals.BASE_URL, WebRequests.Instance.passwordIconEndPoint);

        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        { 
            yield return webRequest.SendWebRequest();
            while(!webRequest.isDone)
                yield return webRequest;
            if (webRequest.isNetworkError)
            { 

            }
            else
            {
                print(webRequest.downloadHandler.text); 
                OnCompletePasswordIcon(JsonUtility.FromJson<PasswordIconResponse>(webRequest.downloadHandler.text));
            } 
        }
  
    }

    void OnCompletePasswordIcon(PasswordIconResponse response)
    {
        print(response.data.password_icons.Count);
    }
}
