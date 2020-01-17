using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable] public class CategoryResponse 
{    public int category_id = 0;
    public int grade_id = 0;
    public string category = "";
    public string category_image = "";
}

[System.Serializable] public class CategoryResponseData: ResponseBase
{
    public CatList data = null;
}
[System.Serializable] public class CatList
{

    public List<CategoryResponse> categories = new List<CategoryResponse>();
} 
public class CategoryManager : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject catItemPrefab;
    [SerializeField] private List<Sprite> itemBGSprite;
    
     
    void Start()
    {
        //PopulateCategoryPanel(Globals.UserLoginDetails.grade);
    }

    public void PopulateCategoryPanel(string _grade)
    { 
        ClearPreviousData();  
        itemIndex = 0;
        HomeMainUIController.EventShowHideLoader.Invoke(true); 
        StartCoroutine(GetCategoryData(_grade, CategoryCallback));
    }
    int itemIndex = 0;
    private void CategoryCallback(CategoryResponseData responce)
    {
        List<CategoryResponse> allCategoryItems = responce.data.categories; 
        if(responce != null && responce.status)
        { 
            dataPopulateCount = allCategoryItems.Count;
            foreach(CategoryResponse item in allCategoryItems)
            {
                GameObject newItem = Instantiate (catItemPrefab, _parent);
                newItem.GetComponent<CategoryItem>().Populate(item.category_id, itemBGSprite[itemIndex++ % itemBGSprite.Count], item.category_image, item.category);
            }
            StartCoroutine(CheckPopulateComplete()); 
        } 
        else
        {
            //Do Nothing
        }
    }
    void ClearPreviousData()
    {   
        for(int i = 0; i < _parent.childCount; i++)
        {
            Destroy(_parent.GetChild(i).gameObject);

        }
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

    IEnumerator GetCategoryData(string grade , System.Action<CategoryResponseData> callback)
    {
        yield return null;
        string endpoint = WebRequests.Instance.categoryEndPoint;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(Globals.BASE_URL + endpoint+grade))
        {
            // Request and wait for the desired page.
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
                callback(JsonUtility.FromJson<CategoryResponseData> (webRequest.downloadHandler.text));
            } 
        }
 

    }



}
