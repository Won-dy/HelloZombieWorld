using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;
    CharacterController cc;
    float gravity = -20f;
    float yVelocity = 0;
    public float jumpPower = 10f;
    public bool isJumping = false;
    public static int inReturnSpace = 0;
    public int hp = 100;
    int maxHp = 100;
    public Slider hpSlider;
    public Text nowHP;
    public float interval = 1f;
    private float time;
    public float starTime = 5.0f;  // 스타 지속 시간
    public static int getItemStar = 0;  // 스타 아이템 획득
    public GameObject starScreen, noHPScreen;
    public GameObject hitEffect;
    Animator anim; //애니메이터 변수

    // Update is called once per frame
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        nowHP.text = "100";
        starScreen.SetActive(false);
        noHPScreen.SetActive(false);
        starTime = 5.0f;
        getItemStar = 0;
        inReturnSpace = 0;
    }
    // Star 먹었을 때, 무적 공간
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Star")
        {
            getItemStar = 1;
            SoundManager.instance.PlayGetHeart();

            Debug.Log("GetStar");
            Destroy(other.gameObject);
        }
        else if (other.tag == "ReturnSpace")
        {
            inReturnSpace = 1;
            Debug.Log("IN ReturnSpace");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ReturnSpace")
        {
            inReturnSpace = 0;
            Debug.Log("OUT ReturnSpace");
        }
    }
    void Update()
    {
        //게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; //이동방향을 설정함
        anim.SetFloat("MoveMotion", dir.magnitude);

        //특정한 이동 벡터를 Transform 컴포넌트가 붙어 있는 게임 오브젝트를 기준으로
        //상대 방향 벡터로 변환해주는 TransformDirection()가 있다.
        dir = Camera.main.transform.TransformDirection(dir);
        if (cc.collisionFlags == CollisionFlags.Below)//지면에 닿아 있는지..
        {
            if (isJumping) // 점프 중이라면
            {
                isJumping = false; // 점프 전 상태로 초기화
            }
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            SoundManager.instance.PlayPlayerJump();
            isJumping = true;
        }

        //캐릭터 수직 속도에 중력 값을 적용한다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
  //      print(yVelocity);
        // 이동 속도에 맞춰 이동한다
        cc.Move(dir * moveSpeed * Time.deltaTime);
        //    transform.position += dir * moveSpeed * Time.deltaTime;

        //현재 플레이어 hp(%)를 hp슬라이더의 value에 반영한다.
        if (hp < 0) hp = 0;
        hpSlider.value = (float)hp / (float)maxHp;
        nowHP.text = string.Format("{0}", hp);

        // *******체력 15 이하******* //
        if (0 < hp && hp <= 15)
        {
            noHPScreen.SetActive(true);
            time += Time.deltaTime;
            if(time >= interval)
            {
                time = 0f;
                SoundManager.instance.PlayPlayerHeart();
            }
        }
        else
            noHPScreen.SetActive(false);

        // *******무적 상태******* //
        if (getItemStar == 1)  // 무적 아이템 먹으면 몇 초간 무적
        {
            if (starTime > 0.0f)  // 무적 상태
            {
                starTime -= Time.deltaTime;
                starScreen.SetActive(true);
            }
            else
            {
                //GameObject.Find("Background").GetComponent<AudioSource>().Play();
                getItemStar = 0;
                starTime = 5.0f;
                starScreen.SetActive(false);
                Debug.Log("스타 시간 끝");
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            hp = 9999;
            hpSlider.value = (float)hp / (float)maxHp;
        }
    }
    public void DamageAction(int damage)
    {
        if(getItemStar == 0) { 
            hp -= damage;
            if (hp > 0)
                StartCoroutine(PlayerHitEffect());
        }
    }
    IEnumerator PlayerHitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitEffect.SetActive(false);
    }
}
