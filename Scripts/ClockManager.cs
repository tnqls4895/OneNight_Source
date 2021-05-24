using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public Text ClockText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int curGameTime = GameEventManager.instance.curGameTime;
        
        
        ClockText.text = "Day " + GameEventManager.instance.currentDay.ToString() + " AM  " +
            (curGameTime / 60).ToString() + ":" + (curGameTime % 60).ToString();

    }
}
