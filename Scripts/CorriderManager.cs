using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CorriderManager : MonoBehaviour
{
    public RectTransform liquidTransform;
    public RectTransform liquid2Transform;
    public RectTransform babyGhostTransform;

    public int currentFloor;

    // Start is called before the first frame update
    void Start()
    {
        currentFloor = GameEventManager.instance.GetCurrentFloor();
    }

    // Update is called once per frame
    void Update()
    {
        CorriderState corriderState = GameEventManager.instance.GetCorriderState(currentFloor);
        liquidTransform.localScale = Vector2.zero;
        liquid2Transform.localScale = Vector2.zero;

        switch(corriderState.currentState)
        {
            case ECorriderState.liquid:
                liquidTransform.localScale = Vector2.one;
                break;
            case ECorriderState.liquid2:
                liquid2Transform.localScale = Vector2.one;
                break;
            case ECorriderState.babyGhost:
                babyGhostTransform.localScale = Vector2.one;
                break;
        }
    }

    public void OnClickedDoor(string roomNumber)
    {
        int room = int.Parse(roomNumber);
        GameEventManager.instance.floor = room / 100;
        GameEventManager.instance.number = room % 10;
        if (room == 302)
        {
            SceneManager.LoadScene("Room302");
        }

        else
        {
            SceneManager.LoadScene("FrontDoor");
        }
    }

    public void OnClickedLiquid()
    {
        GameEventManager.instance.OnClickedCorriderLiquid(currentFloor);
    }

    public void OnClilckedBlood()
    {
        GameEventManager.instance.OnClickedCorriderBlood(currentFloor);
    }
}
