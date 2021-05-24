using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public Transform settingTransform;
    public Transform bgmOnTransform;
    public Transform bgmOffTransform;

    public bool isSettingOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        settingTransform.localScale = Vector2.zero;
        bgmOffTransform.localScale = Vector2.zero;
        bgmOnTransform.localScale = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedSetting()
    {
        if (!isSettingOpen)
        {
            settingTransform.localScale = Vector2.one;
            isSettingOpen = true;
            bgmOnTransform.localScale = Vector2.one;
        }
    }
    //버튼의 글씨들은 현재 브금의 상태
    //On이 써져있는 버튼을 누르면 브금이 꺼지고
    public void OnClickedBgmOn()
    {
        bgmOffTransform.localScale = Vector2.one;
        bgmOnTransform.localScale = Vector2.zero;
        GameEventManager.instance.ClickBgmOff();
    }

    //Off이 써져있는 버튼을 누르면 브금이 켜야됨!
    public void OnClickedBgmOff()
    {
        bgmOffTransform.localScale = Vector2.zero;
        bgmOnTransform.localScale = Vector2.one;
        GameEventManager.instance.ClickBgmOn();
    }

    public void OnClickedClose()
    {
        if (isSettingOpen)
        {
            settingTransform.localScale = Vector2.zero;
            isSettingOpen = false;
        }
    }
}
