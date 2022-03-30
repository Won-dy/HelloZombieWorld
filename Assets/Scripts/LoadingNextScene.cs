using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    int nowStage;
    string whatScene;
    GameObject LoadingManager, txt;
    public Slider loadingBar;  // 로딩 슬라이더 바
    public Text loadingText;  // 로딩 진행 텍스트
    // Start is called before the first frame update
    void Start()
    {
        LoadingManager = GameObject.Find("lts");
        txt = GameObject.Find("Canvas/Ment");
        //nowStage = LoadingManager.GetComponent<LoadingToStage>().startStageNum;
        nowStage = LoadingToStage.startStageNum;
        if (nowStage == 1)
        {
            whatScene = "Stage3Scene";
            txt.GetComponent<Text>().text = "좀비들과 맞서 싸우는 중 . . .";
        }
        else if (nowStage == 2)
        {
            whatScene = "Stage2Scene";
            txt.GetComponent<Text>().text = "좀비 기지 내부로 침입하는 중...";
        }
        Destroy(LoadingManager);
        StartCoroutine(TransitionNextScene(whatScene));
    }
    // 비동기 씬 로드 코루틴
    IEnumerator TransitionNextScene(string sn)
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation ao = SceneManager.LoadSceneAsync(sn);  // 지정된 씬을 비동기 형식으로 로드한다.
        ao.allowSceneActivation = false;  // 로드되는 씬의 모습이 화면에 보이지 않게 한다.

        while (!ao.isDone)
         {
            loadingBar.value = ao.progress;
            loadingText.text = ((int)(ao.progress * 100f)).ToString() + "%";

            if (ao.progress >= 0.9f)  // 진행률 90% 이상
            {
                ao.allowSceneActivation = true;
            }
            print(ao.progress);
            yield return null;
         }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
