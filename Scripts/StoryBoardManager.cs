using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryBoardManager : MonoBehaviour
{
    public RectTransform storyBoardTransform;
    public Text storyText;

    public TextAsset[] storys;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameEventManager.instance.bShowStoryBoard)
        {
            storyBoardTransform.localScale = Vector2.one;
            UpdateText();
        }
        else
        {
            storyBoardTransform.localScale = Vector2.up;
        }
        
    }

    public void OnClickStoryBoard()
    {
        GameEventManager.instance.bShowStoryBoard = false;
    }
    public void UpdateText()
    {
        storyText.text = storys[GameEventManager.instance.currentDay].text;
    }
}
