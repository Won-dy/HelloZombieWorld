using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingToStage : MonoBehaviour
{
    public static int startStageNum;
    //public GameObject startStageNumObject;
    //public static GameObject = GameObject.Find("Canvas/Ment");

    public static void call()
    {
        SceneManager.LoadScene("LoadingScene");
        //DontDestroyOnLoad(startStageNumObject);
    }
}
