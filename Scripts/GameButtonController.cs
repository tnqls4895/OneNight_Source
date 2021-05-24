using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void FloorChange(int currentFloor)
    {
        GameEventManager.instance.currentFloor = currentFloor;
        Debug.Log("floor : " + currentFloor);
    }
}
