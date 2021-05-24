using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonController : MonoBehaviour
{
    public void OnClickedGameStart()
    {
        //타이틀화면실행부터 시간잼
        /*
        GameEventManager.instance.gameStartTime = Time.realtimeSinceStartup;
        GameEventManager.instance.bStartGame = true;*/
        GameEventManager.instance.GameStart();
        SceneManager.LoadScene("LobbyInformation");
    }

    public void OnApplicationQuit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
