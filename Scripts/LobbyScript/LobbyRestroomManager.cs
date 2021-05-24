using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRestroomManager : MonoBehaviour
{

    public RectTransform bloodDoorTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameEventManager.instance.IsEventInvoked(EEventType.bloodReflux))
        {
            bloodDoorTransform.localScale = Vector2.one;
        }
        else
        {
            bloodDoorTransform.localScale = Vector2.up;
        }
    }
}
