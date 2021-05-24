using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerRoomManager : MonoBehaviour
{
    public RectTransform clearShowerBoothTransform;
    public RectTransform dirtyShowerBoothTransform;
    public RectTransform toiletFloor1Transform;
    public RectTransform toiletClosedFloor1Transform;

    public bool bIsOpenToilet = false;
    public bool bIsClearShowerBooth = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameEventManager.instance.IsClearShowerRoom())
        {
            clearShowerBoothTransform.localScale = Vector2.one;
            dirtyShowerBoothTransform.localScale = Vector2.zero;
        }
        else
        {
            clearShowerBoothTransform.localScale = Vector2.zero;
            dirtyShowerBoothTransform.localScale = Vector2.one;
        }

        if (GameEventManager.instance.IsOpenToilet())
        {
            toiletFloor1Transform.localScale = Vector2.one;
            toiletClosedFloor1Transform.localScale = Vector2.up;
        }
        else
        {
            toiletFloor1Transform.localScale = Vector2.up;
            toiletClosedFloor1Transform.localScale = Vector2.one;
        }
    }

    public void OnClickedShowerRoom()
    {
        GameEventManager.instance.OnClickShowerRoom();
    }
    public void OnClickedToilet()
    {
        GameEventManager.instance.OnClickedRoomToilet();
    }
}
