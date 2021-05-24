using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorManager : MonoBehaviour
{
    bool bOccurInterPhoneEvent = false;
    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0,10)<=0)
        {
            bOccurInterPhoneEvent = true;
            SoundFXManager.instance.PlaySound(ESound.ERing);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedInterPhone()
    {
        if (bOccurInterPhoneEvent)
        {
            SoundFXManager.instance.PlaySound(ESound.EUnknownSound);
            bOccurInterPhoneEvent = false;
            GameEventManager.instance.AddWarning(2);
        }
    }
    public void OnClickedDoor()
    {
        GameEventManager.instance.OnClickedFrontDoor();
    }
}
