using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Heart : MonoBehaviour
{
    public int plusHP = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove pm = other.GetComponent<PlayerMove>();
            pm.hp += plusHP;
            if (pm.hp > 100)
                pm.hp = 100;
            SoundManager.instance.PlayGetHeart();

            //GetComponent<AudioSource>().Play();
            //PlayerController.getChargeHP = 1;
            Debug.Log("GetHeart");
            Destroy(gameObject);
        }
    }
}
