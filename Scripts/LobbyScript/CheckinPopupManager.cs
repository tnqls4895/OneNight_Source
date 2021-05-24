using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheckinPopupManager : MonoBehaviour
{
    //체크인팝업창위한 변수
    public RectTransform checkinPopupTransform;
    public RectTransform speechBubbleTransform;
    public RectTransform warningPopupTransform;

    bool bCanShowSpeechBubble = true;
    bool bShowSpeechBubble = false;
    
    public GameObject keys;
    public GameObject[] keyList = new GameObject[12];
    //roomNuber텍스트
    public GameObject roomNumberObject;
    public int checkinRoomNumber;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<12;i++)
        {
            keyList[11-i] = keys.transform.GetChild(i).gameObject;
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<12; i++)
        {
            int floor, number;
            floor = i / 4 + 1;
            number = i % 4 + 1;
            if (GameEventManager.instance.roomStates[floor, number].bCanUseRoom)
            {
                keyList[i].transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                keyList[i].transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }
    public void OnClickedKey(int roomNumber)
    {
        Debug.Log(bShowSpeechBubble);
        if (bShowSpeechBubble)
        {
            roomNumberObject.GetComponent<Text>().text = roomNumber.ToString();
            checkinRoomNumber = roomNumber;
            ShowPopup();
        }
    }
    public void OnClickedPhone()
    {
        if (GameEventManager.instance.IsEventInvoked(EEventType.checkin)&&bCanShowSpeechBubble)
        {
            speechBubbleTransform.localScale = Vector2.one;
            bShowSpeechBubble = true;
        }
    }
    public void ShowPopup()
    {
        bShowSpeechBubble = false;
        speechBubbleTransform.localScale = Vector2.up;
        checkinPopupTransform.localScale = Vector2.one;
    }
    public void HidePopup()
    {
        checkinPopupTransform.localScale = Vector2.zero;
        bCanShowSpeechBubble = true;
    }
    public void HideWarningPopup()
    {
        warningPopupTransform.localScale = Vector2.zero;
    }
    //"네"를 눌렀을때 해당 방 번호를 받아와야하는데 되려나?
    public void OnClickedYes()
    {
        checkinPopupTransform.localScale = Vector2.zero;

        bCanShowSpeechBubble = true;

        int i = checkinRoomNumber / 100;
        int j = checkinRoomNumber % 100;
        //방이 청결상태이고 시체가 없다면
        if(GameEventManager.instance.roomStates[i,j].IsClear() && !GameEventManager.instance.roomStates[i,j].IsGameOverBed() 
            && !GameEventManager.instance.roomStates[i,j].bGhostFrame)
        {
            //팝업창이 사라지고
            checkinPopupTransform.localScale = Vector2.zero;
            //방이 사용불가능으로 바뀌며
            GameEventManager.instance.roomStates[i, j].bCanUseRoom = false;
            //체크인이벤트 상태를 false로 바꾼다
            GameEventManager.instance.beventState[(int)EEventType.checkin] = false;
        }
        //방에 시체가 있다면
        else if(GameEventManager.instance.roomStates[i,j].IsGameOverBed() || GameEventManager.instance.roomStates[i,j].bGhostFrame)
        {
            SoundFXManager.instance.PlaySound(ESound.EScreem);
            GameEventManager.instance.GameOver();
        }
        // 방에 시체가 없고 더러운 상태라면
        else if(!GameEventManager.instance.roomStates[i,j].IsClear())
        {
            //팝업창이 사라지고
            checkinPopupTransform.localScale = Vector2.zero;
            //경고 팝업창이 뜨며
            warningPopupTransform.localScale = Vector2.one;
            //경고 1회를 누적시키고
            GameEventManager.instance.AddWarning(1);
            //체크인이벤트 상태를 false로 바꾼다
            GameEventManager.instance.beventState[(int)EEventType.checkin] = false;
        }
    }
}
