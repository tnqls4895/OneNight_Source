
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletManager : MonoBehaviour
{
    public RectTransform toiletTransform;
    public RectTransform toiletClosedTransform;
    public RectTransform toiletBloodTransform;
    public RectTransform basinBloodTransform;

    //    public RectTransform restroomBloodTransform;

    bool bIsOpenToilet = false;

    bool bIsClickedDetergent = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 만약에 피역류 이벤트가 발생중이라면
        if(GameEventManager.instance.beventState[(int)EEventType.bloodReflux])
        {
            // 기본 변기이미지 가리고
            toiletTransform.localScale = Vector2.zero;
            toiletClosedTransform.localScale = Vector2.zero;
            // 피역류 변기 이미지 보이게 하기
            toiletBloodTransform.localScale = Vector2.one;
            basinBloodTransform.localScale = Vector2.one;
        }
        else // 아니라면
        {
            // 피역류 변기 이미지 가리고
            toiletBloodTransform.localScale = Vector2.zero;
            basinBloodTransform.localScale = Vector2.up;

            if (bIsOpenToilet) // 변기가 열려있으면
            {
                // 열린 변기 보여주고
                toiletTransform.localScale = Vector2.one;
                // 닫힌 변기 가리기
                toiletClosedTransform.localScale = Vector2.zero;
            }
            else //아니라면
            {
                // 닫힌 변기 보여주고
                toiletClosedTransform.localScale = Vector2.one;
                // 열린 변기 가리기
                toiletTransform.localScale = Vector2.zero;
            }
        }
    }
    public void OnClickedToilet()
    {
        if(bIsOpenToilet == true)
        {
            bIsOpenToilet = false;
            toiletTransform.localScale = Vector2.zero;
            toiletClosedTransform.localScale = Vector2.one;
        }
        else if(bIsOpenToilet == false)
        {
            bIsOpenToilet = true;
            toiletClosedTransform.localScale = Vector2.zero;
            toiletTransform.localScale = Vector2.one;
        }
    }
    public void OnClickedBloodToilet()
    {
        if(GameEventManager.instance.beventState[(int)EEventType.bloodReflux] == true && bIsClickedDetergent==true)
        {
            GameEventManager.instance.End_bloodReflux();
        }
    }
    public void OnClickedDetergent()
    {
        if (GameEventManager.instance.beventState[(int)EEventType.bloodReflux] == true && bIsClickedDetergent == false)
        {
            bIsClickedDetergent = true;
        }
    }

}
