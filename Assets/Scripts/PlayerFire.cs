using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition; // 발사 위치
    public GameObject bombFactory; // 수류탄 프리팹
    public float throwPower = 15f;
    public int weaponPower = 5;
    public int headShotPower = 15;
    public static int headShotCnt = 0;  // 헤드샷 변수
    public Text headshotCnt;
    public static int killCnt = 0;  // 킬 변수
    public Text KillCnt;
    public GameObject bulletEffect; // 피격 이펙트 오브젝트
    ParticleSystem ps;//피격 이펙트 파티클 시스템
    public GameObject baamEffect;  // 폭탄 총으로 쏘면 이펙트
    public int nowBullet = 20;  // 총알 UI
    public int maxBullet = 20;
    public Slider bulletSlider;
    public Text nowBL, maxBL;
    public int nowGrenade = 9;  // 수류탄 UI
    public int maxGrenade = 9;
    public Text nowGN;
    public GameObject targetPoint, sniperPoint, sniperFocus;  // 조준경
    Animator anim;
    public GameObject bloodEff;
    public Text wModeText;
    public GameObject[] eff_Flash;
    GameObject skull;
    bool ZoomMode = false;
    private void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();
        sniperPoint.SetActive(false);
        sniperFocus.SetActive(false);
        targetPoint.SetActive(true);
        nowBL.text = "20";
        maxBL.text = "20";
        nowGN.text = "9";
        headShotCnt = 0;
        killCnt = 0; 
    }
    // Update is called once per frame
    void Update()
    {
        //게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // *******Shift 눌렀다면 수류탄******* //
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (nowGrenade > 0 && PlayerMove.inReturnSpace == 0)
            {
                GameObject bomb = Instantiate(bombFactory); // 수류탄 생성
                bomb.transform.position = firePosition.transform.position;
                //수류탄 오브젝트의 Rigidbody 컴포넌트를 가져온다.
                Rigidbody rb = bomb.GetComponent<Rigidbody>();
                //카메라의 정면 방향으로 수류탄에 물리적인 힘을 가한다.
                rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                SoundManager.instance.PlayUseGrenade();
                nowGrenade -= 1;
                sniperFocus.SetActive(false);
            }
        }

        // *******오른쪽 마우스버튼 누르는동안 스나이퍼 모드******* //
        if (!ZoomMode && Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = 15f;
            sniperPoint.SetActive(true);
            sniperFocus.SetActive(true);
            targetPoint.SetActive(false);
            SoundManager.instance.PlayZoomInSniper();
            ZoomMode = true;
        }
        else if (ZoomMode && Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView = 60f;
            sniperPoint.SetActive(false);
            sniperFocus.SetActive(false);
            targetPoint.SetActive(true);
            ZoomMode = false;
        }

        // *******왼쪽 마우스버튼을 눌렀고 잔여 총알이 있고, 무적 공간이 아니라면******* //
        if (Input.GetMouseButtonDown(0) && nowBullet > 0 && PlayerMove.inReturnSpace == 0)
        {
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            nowBullet -= 1;
            SoundManager.instance.PlayShooting();

            //레이를 생성한 후 발사될 위치와 진행 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 좀비가 맞으면
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("ZHead"))
                {
                    EnemyFSM eFSM = hitInfo.transform.parent.parent.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(headShotPower); 
                    skull = hitInfo.transform.GetChild(0).gameObject;
                    StartCoroutine(HeadShootEffectOn(0.7f));
                    GameObject be = Instantiate(bloodEff);
                    be.transform.position = hitInfo.transform.GetChild(0).position;

                    headShotCnt += 1;  // 헤드샷 횟수 변수
                    // *******총 헤드샷 수 갱신******* // 
                    int a = PlayerPrefs.GetInt(LoginManager.LoginID + "HeadShot");  // 총 헤드샷 수
                    a += 1;
                    PlayerPrefs.SetInt(LoginManager.LoginID + "HeadShot", a);  // ++1
                    print("헤드 샷" + headShotCnt);
                }
                else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                    GameObject be = Instantiate(bloodEff);
                    be.transform.position = hitInfo.transform.position;
                    print("그냥 샷");
                }
                // 폭탄에 맞으면
                else if (hitInfo.transform.gameObject.name.Contains("bomb"))
                {
                    SoundManager.instance.PlayTouchBomb();
                    GameObject eff = Instantiate(baamEffect); // 폭발 프리팹 생성
                    eff.transform.position = hitInfo.transform.position;
                    if (hitInfo.transform.gameObject.name == "Old_timer_bomb")
                        hitInfo.transform.parent.gameObject.SetActive(false);
                    else
                        hitInfo.transform.gameObject.SetActive(false);
                }
                // 그 외에 맞으면
                else
                {
                    //피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킴
                    bulletEffect.transform.position = hitInfo.point;
                    //피격 이팩트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다.
                    bulletEffect.transform.forward = hitInfo.normal;
                    ps.Play();
                }
            }
            StartCoroutine(ShootEffectOn(0.05f));
        }

        // *******머즐 플레쉬 랜덤 설정******* //
        IEnumerator ShootEffectOn(float duration)
        {
            // 랜덤하게 숫자를 0~4까지 숫자를 뽑는다.
            int num = Random.Range(0, eff_Flash.Length);
            eff_Flash[num].SetActive(true);
            yield return new WaitForSeconds(duration);
            eff_Flash[num].SetActive(false);
        }

        // *******헤드샷 이펙트 설정******* //
        IEnumerator HeadShootEffectOn(float duration)
        {
            skull.SetActive(true);
            yield return new WaitForSeconds(duration);
            skull.SetActive(false);
        }

        // *******공격 모드 설정******* //
/*        if (Input.GetKeyDown(KeyCode.Alpha1))  // 숫자 키 1을 눌렀다면 노말 모드
        {
            wMode = WeaponMode.Normal;
            Camera.main.fieldOfView = 60f;
            wModeText.text = "Normal Mode";
            sniperFocus.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))  // 숫자 키 2를 눌렀다면 스나이퍼 모드
        {
            wMode = WeaponMode.Sniper;
            wModeText.text = "Sniper Mode";
        }
*/
        // *******총알 장전******* //
        if (Input.GetKeyDown(KeyCode.R) && nowBullet == 0)
        {
            nowBullet = maxBullet;
            SoundManager.instance.PlayChargeBullet();
        }
        if (Input.GetKeyDown(KeyCode.Y))
            nowGrenade = maxGrenade;

        // *******총알 슬라이더 설정******* //
        bulletSlider.value = (float)nowBullet / (float)maxBullet;
        nowBL.text = string.Format("{0}", nowBullet);
        maxBL.text = string.Format("{0}", maxBullet);

        // *******수류탄 설정******* //
        nowGN.text = string.Format("{0}", nowGrenade);

        headshotCnt.text = string.Format("{0}", headShotCnt);
        KillCnt.text = string.Format("{0}", killCnt);
    }
}
