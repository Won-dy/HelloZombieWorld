using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubCamera : MonoBehaviour
{
    public Camera subCmr;
    public GameObject playerPin;
    //public GameObject[] pins;
    //int zbCnt;
/*    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Stage3Scene")
            zbCnt = 18;
        else if (SceneManager.GetActiveScene().name == "Stage2Scene")
            zbCnt = 22;
    }
*/
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            subCmr.gameObject.SetActive(true);
            playerPin.SetActive(true);
/*            for (int i = 0; i < zbCnt; i++)
            {
                pins[i].SetActive(true);
            }
*/        }
        else
        {
            subCmr.gameObject.SetActive(false);
            playerPin.SetActive(false);
/*            for (int i = 0; i < zbCnt; i++)
            {
                pins[i].SetActive(false);
            }
*/        }
    }
}
