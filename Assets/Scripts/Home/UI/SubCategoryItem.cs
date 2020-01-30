using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class SubCategoryItem : MonoBehaviour
{ 
    [SerializeField] private Text titleText;
    [SerializeField] private Image BGImage;
    [SerializeField] private Image subCatIcon;

    private Button clickButton;
    [SerializeField] private int subCatId = 0;
    [SerializeField] private string subCatItemName = "";


    void Start()
    {
     clickButton = GetComponent<Button>();
     clickButton.onClick.AddListener(OnClicked);   
    }

    public void Populate(int _subCatId, string _tilte, string iconUrl, Sprite BGSprite)
    {
        titleText.text = _tilte.ToUpper();
        BGImage.sprite = BGSprite;
        subCatId = _subCatId;
        subCatItemName = _tilte;
        StartCoroutine(SetIconUrl(iconUrl));
    }  

    IEnumerator SetIconUrl(string _iconUrl)
    {
       UnityWebRequest www = UnityWebRequestTexture.GetTexture(_iconUrl);
        yield return www.SendWebRequest();
        while(!www.isDone)
        yield return null;

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {

            Texture2D myTexture = (Texture2D)((DownloadHandlerTexture)www.downloadHandler).texture;
            subCatIcon.sprite =  Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

    }

    void OnClicked()
    {
        HomeMainUIController.SubCatClicked.Invoke(subCatId, subCatItemName);
    }
   
}
