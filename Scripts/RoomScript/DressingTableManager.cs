using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressingTableManager : MonoBehaviour
{
    public RectTransform closeLeftDrawer;
    public RectTransform openLeftDrawer;
    public RectTransform closeRightDrawer;
    public RectTransform openRightDrawer;

    public RectTransform ghostFrame;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameEventManager.instance.IsLeftDrawerOpen())
        {
            openLeftDrawer.localScale = Vector2.one;
            closeLeftDrawer.localScale = Vector2.up;
        }
        else
        {
            openLeftDrawer.localScale = Vector2.up;
            closeLeftDrawer.localScale = Vector2.one;
        }
        if (GameEventManager.instance.IsRightDrawerOpen())
        {
            openRightDrawer.localScale = Vector2.one;
            closeRightDrawer.localScale = Vector2.up;
        }
        else
        {
            openRightDrawer.localScale = Vector2.up;
            closeRightDrawer.localScale = Vector2.one;
        }
        if (GameEventManager.instance.IsGhostFrame())
        {
            ghostFrame.localScale = Vector2.one;
        }
        else
        {
            ghostFrame.localScale = new Vector2(1, 0);
        }
    }


    public void OnClickedLeftDrawer()
    {
        GameEventManager.instance.OnClickedLeftDrawer();
    }

    public void OnClickedRightDrawer()
    {
        GameEventManager.instance.OnClickedRightDrawer();
    }

    public void OnClickedGhostFrame()
    {
        GameEventManager.instance.OnClickedGhostFrame();
    }
}
