using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zombie_State : MonoBehaviour
{
    public static int isClear = 0;
    public int zb_Normal, zb_Red, zb_Blue;
    public Text[] nowZombieNum;
    private void Start()
    {
        isClear = 0;
    }
    void Update()
    {
        nowZombieNum[0].text = string.Format("{0}", zb_Normal);
        nowZombieNum[1].text = string.Format("{0}", zb_Red);
        nowZombieNum[2].text = string.Format("{0}", zb_Blue);

        // *******좀비 다 죽임******* //
        if (zb_Normal == 0 && zb_Red == 0 && zb_Blue == 0) isClear = 1;
        else isClear = 0;

        if (Input.GetKeyDown(KeyCode.U))
            isClear = 1;
    }
}
