using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text[] TimerText;
    float time = 0f;
    public static int totTime = 0;

    void Start()
    {
        time = 0f;
        totTime = 0;
    }
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
            return;
        time += Time.deltaTime;
        TimerText[0].text = string.Format("{0:00}", (int)time / 60 % 60);
        TimerText[1].text = string.Format("{0:00}", (int)time % 60);
        TimerText[2].text = string.Format("{0:00}", (int)time / 60 % 60);
        TimerText[3].text = string.Format("{0:00}", (int)time % 60);
        totTime = (int)time;
    }
}
