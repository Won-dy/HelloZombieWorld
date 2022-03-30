using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static int stageNum;
    public static void nowScene()
    {
        if (SceneManager.GetActiveScene().name == "Stage3Scene") stageNum = 1;
        else if (SceneManager.GetActiveScene().name == "Stage2Scene") stageNum = 2;

/*        if (stageNum != 0)
        {
            SceneManager.LoadScene("HomeScene");
            DontDestroyOnLoad(GameObject.Find("StageName"));
        }
*/    }
}
