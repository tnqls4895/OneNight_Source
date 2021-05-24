using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorManager : MonoBehaviour
{
    public RectTransform basementButtonTransform;
    public RectTransform emergencyBellButtonTransform;
    public Text floorText;
    public int icurrentFloor = 0;

    bool bBasementButton = false;
    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0,10) <= 2)
        {
            bBasementButton = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(bBasementButton)
        {
            basementButtonTransform.localScale = Vector2.one;
            emergencyBellButtonTransform.localScale = Vector2.up;
        }
        else
        {
            basementButtonTransform.localScale = Vector2.up;
            emergencyBellButtonTransform.localScale = Vector2.one;
        }

        icurrentFloor = GameEventManager.instance.GetCurrentFloor();

        //엘레베이터에 현재 층수 출력
        if (icurrentFloor == 0)
        {
            floorText.text = "L";
        }
        else
        {
            floorText.text = icurrentFloor.ToString();
        }

    }
    public void OnClickedBasementButton()
    {
        if(bBasementButton)
        {
            GameEventManager.instance.GameOver();
        }
    }
}
