using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadingManager : MonoBehaviour
{
    //public LoadingToStage ltc;
    public void OnClickBox()
    {
        string nowbutton = EventSystem.current.currentSelectedGameObject.name;
        if (nowbutton == "Stage1") LoadingToStage.startStageNum = 1;
        else if (nowbutton == "Stage2") LoadingToStage.startStageNum = 2;

        if (LoadingToStage.startStageNum != 0) LoadingToStage.call();
    }
}
