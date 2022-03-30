using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Grenade : MonoBehaviour
{
    public int plusGrenade = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerFire pf = other.GetComponent<PlayerFire>();
            pf.nowGrenade += plusGrenade;
            if (pf.nowGrenade > 9)  // 수류탄 최대 개수 제한
                pf.nowGrenade = 9;
            SoundManager.instance.PlayGetBullet();

            Debug.Log("GetGrenade");
            Destroy(gameObject);
        }
    }
}
