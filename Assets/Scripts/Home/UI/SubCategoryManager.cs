using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


[Serializable] public class CategoryResponseSub
{
public int sub_category_id = 0;
public int category_id = 0;
public string sub_category = "";
public string sub_category_image = "";
public string category_name = "";

} 

[Serializable] public class SubCatList
{
    public List<CategoryResponseSub> sub_categories = new List<CategoryResponseSub>();
}
[Serializable] public class SubCategoryResponseData: ResponseBase
{
    public SubCatList data = null;
} 

public class SubCategoryManager : MonoBehaviour
{
    [SerializeField] List<Sprite> subCatBG;
    [SerializeField] Text title;
     
    [SerializeField] GameObject subCatPrefab;
    [SerializeField] Transform _parent;

    #region Monobehaviour
    void Start()
    {

    }

    #endregion Monobehaviour

    public void Populate(int _catId, string _title)
    {
        ClearPreviousData();
        HomeMainUIController.EventShowHideLoader.Invoke(true);
        title.text = _title.ToUpper();
        StartCoroutine(GetSubCategory(_catId, SubCategoryCallback));
    }

    IEnumerator GetSubCategory(int _catId, Action<SubCategoryResponseData> callback)
    {
         
        string endpoint = WebRequests.Instance.subCategoryEndPoint;
        print("request for cat id "+_catId);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Globals.BASE_URL + endpoint+_catId))
        {
            // Request and wait for the desired page.
            print("URL "+webRequest.url);
            webRequest.SetRequestHeader("Accept", "application/json");//
            webRequest.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);

            yield return webRequest.SendWebRequest();
            while(!webRequest.isDone)
                yield return webRequest;
            if (webRequest.isNetworkError)
            {
              callback(null);  
            }
            else
            {
                print(webRequest.downloadHandler.text);
                callback(JsonUtility.FromJson<SubCategoryResponseData> (webRequest.downloadHandler.text));
            }
        } 

    }
int itemIndex = 0;
    void SubCategoryCallback(SubCategoryResponseData response)
    {
          
        if(response != null)
        {
            
            if(response.status)
            {
                 
                dataPopulateCount = response.data.sub_categories.Count;
                itemIndex = 0;
                print("dataPopulateCount "+dataPopulateCount);
                foreach(CategoryResponseSub item  in response.data.sub_categories)
                {
                    GameObject newItem = Instantiate(subCatPrefab, _parent);
                    newItem.GetComponent<SubCategoryItem>().Populate(item.sub_category_id, item.sub_category, item.sub_category_image, subCatBG[itemIndex++ % subCatBG.Count]);

                }
            }
        }
        StartCoroutine(CheckPopulateComplete()); 

    }

    int dataPopulateCount = 0;
    IEnumerator CheckPopulateComplete()
    {
        while(_parent.childCount < dataPopulateCount)
        {
            yield return null;
        }
        HomeMainUIController.EventShowHideLoader.Invoke(false);  
    }

    void ClearPreviousData()
    {   
        for(int i = 0; i < _parent.childCount; i++)
        {
            Destroy(_parent.GetChild(i).gameObject);
        }
    }
    
}
