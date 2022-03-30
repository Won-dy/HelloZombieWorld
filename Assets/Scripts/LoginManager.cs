using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField id;
    public InputField pw;
    public Text notify;
    public static string LoginID;
    // Start is called before the first frame update
    void Start()
    {
        notify.text = "";
        // 랭킹 변수 초기화
        if (!PlayerPrefs.HasKey("Stage1Rank1ID"))  
        {   print("생성");
            for(int i = 1; i <= 2; i++)
            {
                for(int j = 1; j <= 5; j++)
                {
                    PlayerPrefs.SetString("Stage" + i + "Rank" + j + "ID", "-");
                    PlayerPrefs.SetInt("Stage" + i + "Rank" + j + "Time", 5999);
                }
            }
        }
    }
    public void SaveUserData()
    {
        if (!CheckInput(id.text, pw.text)) return;

        if (!PlayerPrefs.HasKey(id.text)) {
            PlayerPrefs.SetString(id.text, pw.text);
            PlayerPrefs.SetInt(id.text + "Stage1", 5999);  // Stage1 최고기록 초
            PlayerPrefs.SetInt(id.text + "Stage2", 5999);  // Stage2 최고기록 초
            PlayerPrefs.SetInt(id.text + "Kill", 0);  // 총 죽인 좀비 수
            PlayerPrefs.SetInt(id.text + "HeadShot", 0);  // 총 헤드샷 수
            notify.text = "계정 생성 성공";
        }
        else
            notify.text = "계정 생성 실패: 이미 존재하는 ID";
    }
    public void CheckUserData()  // 로그인 함수
    {
        if (!CheckInput(id.text, pw.text)) return;
        string pass = PlayerPrefs.GetString(id.text);
        if (pw.text == pass) 
        {
            LoginID = id.text;
            SceneManager.LoadScene(1);
        }
        else
            notify.text = "입력한 ID 또는 PW가 일치하지 않음";
    }
    bool CheckInput(string id, string pw)
    {
        if (id == "" || pw == "")
        {
            notify.text = "ID 또는 PW를 입력해주세요";
            return false;
        }
        else return true;
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
