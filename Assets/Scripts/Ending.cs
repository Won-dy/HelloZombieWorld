using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public Text thankMSG;
    string nowID = LoginManager.LoginID;
    private void Start()
    {
        thankMSG.text = "Thanks to, " + nowID + " :)";
    }
    public void GoHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
