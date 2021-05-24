using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorManager : MonoBehaviour
{
    bool bDoorKnock = false;
    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0,10) <= 0)
        {
            bDoorKnock = true;
            SoundFXManager.instance.PlaySound(ESound.EDoorKnock);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedExitDoor()
    {
        //문 열리는 이미지 바뀌는 코드 넣어야됨

        if (bDoorKnock)
        {
            GameEventManager.instance.GameOver();
        }
    }
}
