using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SpeedPotion : MonoBehaviour
{

    public int plusSpeed = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMove pm = other.GetComponent<PlayerMove>();
            Debug.Log("BF " + pm.moveSpeed);
            pm.moveSpeed += plusSpeed;
            SoundManager.instance.PlayGetPotion();
            
            Debug.Log("AF " + pm.moveSpeed);
            //GameObject p = transform.parent.gameObject;
            //p.transform.parent.gameObject.SetActive(false);
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
