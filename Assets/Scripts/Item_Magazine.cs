using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Magazine : MonoBehaviour
{
    public int plusMagazine = 5;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerFire pf = other.GetComponent<PlayerFire>();
            pf.maxBullet += plusMagazine;
            SoundManager.instance.PlayGetBullet();

            Debug.Log("GetMagazine");
            Destroy(gameObject);
        }
    }
}
