using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public Text hello1, hello2, nowID;
    public Text[] RecordText;
    public Text[] Ranking1ID, Ranking2ID, Ranking1Time, Ranking2Time;
    public GameObject Ranking, Record, HowTo;
    int stgT1, stgT2;
    // Start is called before the first frame update
    void Start()
    {
        hello1.text = "Hello, " + LoginManager.LoginID;
        hello2.text = "Hello, " + LoginManager.LoginID;
        nowID.text = LoginManager.LoginID;
        stgT1 = PlayerPrefs.GetInt(LoginManager.LoginID + "Stage1");
        stgT2 = PlayerPrefs.GetInt(LoginManager.LoginID + "Stage2");

        if (stgT1 == 5999)
            RecordText[0].text = "--:--";
        else
        {
            string str1 = string.Format("{0:00}", stgT1 / 60);
            string str2 = string.Format("{0:00}", stgT1 % 60);
            RecordText[0].text = str1 + ":" + str2;
        }

        if (stgT2 == 5999)
            RecordText[1].text = "--:--";
        else
        {
            string str3 = string.Format("{0:00}", stgT2 / 60);
            string str4 = string.Format("{0:00}", stgT2 % 60);
            RecordText[1].text = str3 + ":" + str4;
        }

        RecordText[2].text = PlayerPrefs.GetInt(LoginManager.LoginID + "Kill").ToString();
        RecordText[3].text = PlayerPrefs.GetInt(LoginManager.LoginID + "HeadShot").ToString();

        for (int i = 1; i <= 2; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                string min = string.Format("{0:00}", PlayerPrefs.GetInt("Stage" + i + "Rank" + j + "Time") / 60);
                string sec = string.Format("{0:00}", PlayerPrefs.GetInt("Stage" + i + "Rank" + j + "Time") % 60);

                if (i == 1)
                {
                    Ranking1Time[j - 1].text = min + ":" + sec;
                    Ranking1ID[j - 1].text = PlayerPrefs.GetString("Stage" + i + "Rank" + j + "ID");
                    if (PlayerPrefs.GetInt("Stage" + i + "Rank" + j + "Time") == 5999)
                        Ranking1Time[j - 1].text = "--:--";
                }
                else if (i == 2)
                {
                    Ranking2ID[j - 1].text = PlayerPrefs.GetString("Stage" + i + "Rank" + j + "ID");
                    Ranking2Time[j - 1].text = min + ":" + sec;
                    if (PlayerPrefs.GetInt("Stage" + i + "Rank" + j + "Time") == 5999)
                        Ranking2Time[j - 1].text = "--:--";
                }
            }
        }
    }
        
    public void ClickStage1()
    {

        SceneManager.LoadScene("LoadingScene");
    }
    public void ClickStage2()
    {

        SceneManager.LoadScene("LoadingScene");
    }
    public void ClickMyRecord()
    {
        Ranking.SetActive(false);
        Record.SetActive(true);
        HowTo.SetActive(false);
    }
    public void ClickRanking()
    {
        Ranking.SetActive(true);
        Record.SetActive(false);
        HowTo.SetActive(false);
    }
    public void ClickHowTo()
    {
        HowTo.SetActive(true);
        Ranking.SetActive(false);
        Record.SetActive(false);
    }
    public void ClickCloseMyRecord()
    {
        Record.SetActive(false);
    }
    public void ClickCloseRanking()
    {
        Ranking.SetActive(false);
    }
    public void ClickCloseHowTo()
    {
        HowTo.SetActive(false);
    }

    public void ClickLogout()
    {

        SceneManager.LoadScene("LoginScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
