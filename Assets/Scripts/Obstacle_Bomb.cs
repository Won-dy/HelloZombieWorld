using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Bomb : MonoBehaviour
{
    public GameObject baamEffect;
    public int MinusHP = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PlayerMove.getItemStar == 0)
        {
            PlayerMove pm = other.GetComponent<PlayerMove>();
            pm.hp -= MinusHP;
            SoundManager.instance.PlayTouchBomb();
            GameObject eff = Instantiate(baamEffect); // 폭발 프리팹 생성
            eff.transform.position = transform.position;

            Debug.Log("GetBomb");
            if(name == "Old_timer_bomb")
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
    }
}
