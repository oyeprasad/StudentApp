using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{ 
//inside class
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    [SerializeField] private List<GameObject> allPanels;
    private int currIndex = 0;
    AsyncOperation asyncLoad;
    void Start()
    {
        asyncLoad = SceneManager.LoadSceneAsync(Globals.LOGIN_SCENE, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;     
    }
    void Update()
    {
    Swipe();   
    } 
    public void Swipe()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
        if(Input.GetMouseButtonUp(0))
        {
                //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        
                //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            
            //normalize the 2d vector
            currentSwipe.Normalize();
    
            //swipe upwards
            if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Debug.Log("up swipe");
            }
            //swipe down
            if(currentSwipe.y < 0 &&  currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Debug.Log("down swipe");
            }
            //swipe left
            if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                Debug.Log("left swipe");
                OnSwipeRight();
            }
            //swipe right
            if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                Debug.Log("right swipe");
                OnSwiprLeft();
            }
        }
    }
    
    private void OnSwiprLeft()
    {
        
        if(currIndex > 0)
        {
            currIndex -= 1;
            ActivatePanel(currIndex);
        }

    }

    public void SkipClicked()
    {
        asyncLoad.allowSceneActivation = true;
        //Globals.LoadLevel(Globals.LOGIN_SCENE);
    }
    public void NextClicked()
    {
        OnSwipeRight();
    }

    private void OnSwipeRight()
    {
        if(currIndex < allPanels.Count-1)
        {
            currIndex += 1;
            ActivatePanel(currIndex);
            print(currIndex);
        } else
        {
            asyncLoad.allowSceneActivation = true;
            //Globals.LoadLevel(Globals.LOGIN_SCENE);
        }
    }

    void ActivatePanel(int index)
    {
        for(int i = 0; i < allPanels.Count; i++)
        {
            if(i == index)
                allPanels[i].SetActive(true);
            else
                 allPanels[i].SetActive(false);
        }
    }
}