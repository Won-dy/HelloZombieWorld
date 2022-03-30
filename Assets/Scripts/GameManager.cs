using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerMove player; //플레이어의 체력을 가져오기 위함
    public static GameManager gm;
    public GameObject gameLabel;
    public GameObject clearPanel;
    Text gameText;
    int cnt = 0;
    private void Awake()
    {
        if (gm == null) gm = this;
    }
    public enum GameState
    {
        Ready, Run, Pause, GameOver, Clear
    }
    public GameState gState;
    public GameObject gameOption;  // 옵션 화면 UI 오브젝트 변수
    // Start is called before the first frame update
    void Start()
    {
        //초기 게임 상태는 준비 상태로 설정한다.
        gState = GameState.Ready;
        //게임 상태 UI오브젝트에서 Text 컴포넌트를 가져온다.
        gameText = gameLabel.GetComponent<Text>();
        gameText.alignment = TextAnchor.MiddleCenter;
        //상태 텍스트의 내용을 Ready...로 한다.
        gameText.text = "Ready...";
        //상태 텍스트의 색상을 주황색으로 한다.
        gameText.color = new Color32(255, 185, 0, 255);
        //게임 준비->게임 중 상태로 전환하기
        //StopAllCoroutines();
        StartCoroutine(ReadyToStart());
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        cnt = 0;
    }
    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.alignment = TextAnchor.MiddleCenter;
        gameText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        gameLabel.SetActive(false);
        gState = GameState.Run;
        //StopCoroutine(ReadyToStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.hp <= 0)  // 플레이어가 죽었다면
        {
            if (cnt == 0)
            { 
                SoundManager.instance.PlayPlayerDie();
                cnt++;
            }
            //플레이어 애니메이션을 멈춘다. 
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0);
            gameLabel.SetActive(true);
            gameText.alignment = TextAnchor.UpperCenter;
            gameText.text = "Game Over";
            gameText.color = new Color32(255, 0, 0, 255);
            // 상태 텍스트의 자식 오브젝트의 트렌스폼 컴포넌트를 가져온다.
            Transform buttons = gameText.transform.GetChild(0);
            buttons.gameObject.SetActive(true);

            gState = GameState.GameOver;
        }
        // *******클리어******* //
        if (Zombie_State.isClear == 1)
        {
            clearPanel.SetActive(true);
            Time.timeScale = 1f;
            // *******최고기록 갱신******* 
            if(SceneManager.GetActiveScene().name == "Stage3Scene") { 
                float bestT = PlayerPrefs.GetInt(LoginManager.LoginID + "Stage1");
                print("1 현재 걸린 시간" + Timer.totTime + "원래 최초" + bestT);
                if(Timer.totTime < bestT)  // 계정의 최고기록이면
                {   // 계정 최고 기록 갱신
                    PlayerPrefs.SetInt(LoginManager.LoginID + "Stage1", Timer.totTime);
                    UpdateRanking(Timer.totTime, 1);  // 랭킹 갱신
                }
            } else if(SceneManager.GetActiveScene().name == "Stage2Scene")
            {
                float bestT = PlayerPrefs.GetInt(LoginManager.LoginID + "Stage2");
                print("2 현재 걸린 시간" + Timer.totTime + "원래 최초" + bestT);
                if (Timer.totTime < bestT)
                {   
                    PlayerPrefs.SetInt(LoginManager.LoginID + "Stage2", Timer.totTime);
                    UpdateRanking(Timer.totTime, 2);  // 랭킹 갱신
                }
            }
            gState = GameState.Clear;
        }

        // *******게임 옵션******* //
        if (Input.GetKeyDown(KeyCode.Q))
            OpenOptionWindow();
    }
    public void UpdateRanking(int newTime, int stageNum)
    {
        for(int i = 1; i <= 5; i++)
        {
            // i등 보다 더 최고기록이면
            if(newTime < PlayerPrefs.GetInt("Stage" + stageNum + "Rank" + i + "Time"))
            {
                for(int j = 6 - i; j > 0; j--)  // 랭킹 갱신
                {
                    PlayerPrefs.SetInt("Stage" + stageNum + "Rank" + (j + 1) + "Time",
                        PlayerPrefs.GetInt("Stage" + stageNum + "Rank" + j + "Time"));
                    PlayerPrefs.SetString("Stage" + stageNum + "Rank" + (j + 1) + "ID",
                        PlayerPrefs.GetString("Stage" + stageNum + "Rank" + j + "ID"));
                }
                PlayerPrefs.SetInt("Stage" + stageNum + "Rank" + i + "Time", newTime);
                PlayerPrefs.SetString("Stage" + stageNum + "Rank" + i + "ID", LoginManager.LoginID);
                break;
            }
        }
    }
    public void OpenOptionWindow()
    {
        gameOption.SetActive(true);  // 옵션 창을 활성화 한다.
        Time.timeScale = 0f;  // 게임 속도를 0배속으로 전환한다.
        gState = GameState.Pause;  // 게임 상태를 일시정지 상태로 변경
    }
    public void CloseOptionWindow()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1f;
        gState = GameState.Run;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (SceneManager.GetActiveScene().name == "Stage3Scene")
        {
            LoadingToStage.startStageNum = 1;
            LoadingToStage.call();
        }
        //SceneManager.LoadScene("Stage3Scene");
        else if (SceneManager.GetActiveScene().name == "Stage2Scene")
        {
            LoadingToStage.startStageNum = 2;
            LoadingToStage.call();
        }
        //SceneManager.LoadScene("Stage2Scene");
    }
    public void NextStage()
    {
        if (SceneManager.GetActiveScene().name == "Stage3Scene")
        {
            LoadingToStage.startStageNum = 2;
            LoadingToStage.call();
        }
        //SceneManager.LoadScene("Stage3Scene");
        else if (SceneManager.GetActiveScene().name == "Stage2Scene")
        {
            SceneManager.LoadScene("EndingScene");
        }
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
    }
}
