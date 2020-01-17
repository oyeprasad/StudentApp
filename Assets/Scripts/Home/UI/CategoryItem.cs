using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
public class CategoryItem : MonoBehaviour
{
    [SerializeField] Text catTitle;
    [SerializeField] Image catImage;
    [SerializeField] Image catBackground;

    private Button clickButton;
    private int category_id;
    private int grade_id;
    private string categoryName;

    void Start()
    {
        clickButton = GetComponent<Button>();
        clickButton.onClick.AddListener(OnClicked);
    }
    public void Populate(int _catId, Sprite _bgIcon, string _catImageUrl, string _title)
    { 
        category_id = _catId;
        catBackground.sprite = _bgIcon;
      //  catImage.sprite = _catImage;
        catTitle.text = _title.ToUpper();
        StartCoroutine(SetBGFromUrl(_catImageUrl));
        categoryName = _title;
    }
 IEnumerator SetBGFromUrl(string imageUrl)
 {
    UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();
        while(!www.isDone)
        yield return null;

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {

            Texture2D myTexture = (Texture2D)((DownloadHandlerTexture)www.downloadHandler).texture;
            catImage.sprite =  Sprite.Create(myTexture, new Rect(0.0f, 0.0f, 200, 200), new Vector2(0.5f, 0.5f), 100.0f);
        }

 }         
    
 void OnClicked()
 {
     HomeMainUIController.EventCategoryItemClicked.Invoke(category_id, categoryName);
 }

}
