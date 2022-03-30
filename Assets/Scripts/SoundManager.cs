using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource myAudio;
    public AudioClip getHeartSound, getPotionSound, getBulletSound, touchBombSound, 
        useGrenadeSound, zoomInSniperSound, chargeBulletSound, shootingSound, baamGrenadeSound,
        PlayerWorkSound, PlayerJumpSound, PlayerHeartSound, PlayerDieSound, 
        ZombieMoveSound, ZombieDieSound, ZombieAttackSound;
    public static SoundManager instance;
    void Awake()
    {
        if (SoundManager.instance == null)
            SoundManager.instance = this;
    }
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    public void PlayPlayerHeart()
    {
        myAudio.PlayOneShot(PlayerHeartSound);
    }
    public void PlayPlayerDie()
    {
        myAudio.PlayOneShot(PlayerDieSound);
    }
    public void PlayPlayerWork()
    {
        myAudio.PlayOneShot(PlayerWorkSound);
    }
    public void PlayPlayerJump()
    {
        myAudio.PlayOneShot(PlayerJumpSound);
    }
    public void PlayBaamGrenade()
    {
        myAudio.PlayOneShot(baamGrenadeSound);
    }
    public void PlayShooting()
    {
        myAudio.PlayOneShot(shootingSound);
    }
    public void PlayChargeBullet()
    {
        myAudio.PlayOneShot(chargeBulletSound);
    }
    public void PlayGetHeart()
    {
        myAudio.PlayOneShot(getHeartSound);
    }
    public void PlayGetPotion()
    {
        myAudio.PlayOneShot(getPotionSound);
    }
    public void PlayGetBullet()
    {
        myAudio.PlayOneShot(getBulletSound);
    }
    public void PlayTouchBomb()
    {
        myAudio.PlayOneShot(touchBombSound);
    }
    public void PlayUseGrenade()
    {
        myAudio.PlayOneShot(useGrenadeSound);
    }
    public void PlayZoomInSniper()
    {
        myAudio.PlayOneShot(zoomInSniperSound);
    }
    public void PlayZombieMove()
    {
        myAudio.PlayOneShot(ZombieMoveSound);
    }
    public void PlayZombieDie()
    {
        myAudio.PlayOneShot(ZombieDieSound);
    }
    public void PlayZombieAttack()
    {
        myAudio.PlayOneShot(ZombieAttackSound);
    }
}
