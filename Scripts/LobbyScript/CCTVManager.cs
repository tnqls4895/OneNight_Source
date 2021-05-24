using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECCTVScreen
{
//    Outside,
    floor1Corrider,
    floor2Corrider,
    floor3Corrider,
    LobbyElavator,
    LobbyExit,
    LobbyInformation,
    LobbyRestroom,

    max
}
public class CCTVManager : MonoBehaviour
{
    public RectTransform[] CCTVScreens = new RectTransform[(int)ECCTVScreen.max];
    public RectTransform CCTVScreen;

    public RectTransform floor1Liquid;
    public RectTransform floor1Liquid2;

    public RectTransform floor2Liquid;
    public RectTransform floor2Liquid2;

    public RectTransform floor3Liquid;
    public RectTransform floor3Liquid2;

    public RectTransform bloodDoor;

    public ECCTVScreen currentScreen;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<(int)ECCTVScreen.max; i++)
        {
            CCTVScreens[i] = CCTVScreen.GetChild(i).GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < (int)ECCTVScreen.max; i++)
            CCTVScreens[i].localScale = Vector2.up;
        CCTVScreens[(int)currentScreen].localScale = Vector2.one;

        switch(currentScreen)
        {
            /*
            case ECCTVScreen.Outside:
                UpdateOutSide();
                break;
                */
            case ECCTVScreen.floor1Corrider:
                UpdateFloor1Corrider();
                break;
            case ECCTVScreen.floor2Corrider:
                UpdateFloor2Corrider();
                break;
            case ECCTVScreen.floor3Corrider:
                UpdateFloor3Corrider();
                break;
            case ECCTVScreen.LobbyElavator:
                UpdateLobbyElavator();
                break;
            case ECCTVScreen.LobbyExit:
                UpdateLobbyExit();
                break;
            case ECCTVScreen.LobbyRestroom:
                UpdateLobbyRestroom();
                break;
            case ECCTVScreen.LobbyInformation:
                UpdateLobbyInformation();
                break;
        }
    }

    void UpdateOutSide()
    {
        // 손님 추가
    }
    void UpdateFloor1Corrider()
    {
        CorriderState corriderState = GameEventManager.instance.GetCorriderState(1);
        floor1Liquid.localScale = Vector2.zero;
        floor1Liquid2.localScale = Vector2.zero;
        if(corriderState.currentState==ECorriderState.liquid)
        {
            floor1Liquid.localScale = Vector2.one;
        }
        else if(corriderState.currentState == ECorriderState.liquid2)
        {
            floor1Liquid2.localScale = Vector2.one;
        }
    }
    void UpdateFloor2Corrider()
    {
        CorriderState corriderState = GameEventManager.instance.GetCorriderState(2);
        floor2Liquid.localScale = Vector2.zero;
        floor2Liquid2.localScale = Vector2.zero;
        if (corriderState.currentState == ECorriderState.liquid)
        {
            floor2Liquid.localScale = Vector2.one;
        }
        else if (corriderState.currentState == ECorriderState.liquid2)
        {
            floor2Liquid2.localScale = Vector2.one;
        }
    }
    void UpdateFloor3Corrider()
    {
        CorriderState corriderState = GameEventManager.instance.GetCorriderState(3);
        floor3Liquid.localScale = Vector2.zero;
        floor3Liquid2.localScale = Vector2.zero;
        if (corriderState.currentState == ECorriderState.liquid)
        {
            floor3Liquid.localScale = Vector2.one;
        }
        else if (corriderState.currentState == ECorriderState.liquid2)
        {
            floor3Liquid2.localScale = Vector2.one;
        }
    }
    void UpdateLobbyElavator()
    {

    }
    void UpdateLobbyExit()
    {

    }
    void UpdateLobbyRestroom()
    {
        if(GameEventManager.instance.IsEventInvoked(EEventType.bloodReflux))
        {
            bloodDoor.localScale = Vector2.one;
        }
        else
        {
            bloodDoor.localScale = Vector2.up;
        }
    }
    void UpdateLobbyInformation()
    {

    }

    public void OnClickedLeftScreenButton()
    {
        currentScreen--;
        if (currentScreen < 0)
            currentScreen = ECCTVScreen.max - 1;
    }
    public void OnClickedRightScreenButton()
    {
        currentScreen++;
        if (currentScreen == ECCTVScreen.max)
            currentScreen = 0;
//            currentScreen = ECCTVScreen.Outside;
    }

}
