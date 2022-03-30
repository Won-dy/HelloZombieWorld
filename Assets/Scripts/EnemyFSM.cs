using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    //에너미 상태 상수
    enum EnemyState
    {
        Idle, Move, Attack, Return, Damaged, Die
    }
    EnemyState m_State;
    public float findDistance = 8f; // 플레이어 발견 범위
    Transform player; // 플레이어 트랜스폼
    public float attackDistance = 2f; //공격 가능범위
    public float moveSpeed = 5f;
    CharacterController cc;
    float currentTime = 0; // 누적시간 
    float attackDelay = 2f; // 공격 딜레이 시간
    public int attackPower = 10;
    Vector3 originPos; // 초기 위치 저장용 변수
    Quaternion originRot; 
    public float moveDistance = 10f; //최대 이동가능 범위
    public int hp = 15;
    public int maxHp = 15;
    public Slider hpSlider;
    public GameObject dieEff;
    Animator anim;
    NavMeshAgent smith;
    Zombie_State zs;
    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;
        player = GameObject.Find("Player").transform; // 플레이어의 트랜스폼 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
        originPos = transform.position;
        originRot = transform.rotation;
        anim = transform.GetComponentInChildren<Animator>();
        smith = GetComponent<NavMeshAgent>();
        zs = GameObject.Find("TOP Bar").GetComponent<Zombie_State>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
           case EnemyState.Idle: Idle(); break;
           case EnemyState.Move: Move(); break;
           case EnemyState.Attack: Attack(); break;
           case EnemyState.Return: Return(); break;
           case EnemyState.Damaged: //Damaged();
                break;
           case EnemyState.Die:// Die();
                break;
                 
        }
        hpSlider.value = (float)hp / (float)maxHp;
    }

    void Idle()
    {
        //플레이어와의 거리가 범위 이내라면 Move 상태로 전환
        if (Vector3.Distance(transform.position, player.position) < findDistance && PlayerMove.inReturnSpace == 0)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle->Move");
            //이동 애니메이션으로 전환하기
            anim.SetTrigger("IdleToMove");
            SoundManager.instance.PlayZombieMove();
        }
    }
    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance || PlayerMove.inReturnSpace == 1)
        {
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        //플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동한다
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)// && PlayerMove.inReturnSpace == 0)
        {
            /*
            //이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized;
            //이동
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.forward = dir;
            */
            if (PlayerMove.inReturnSpace == 0)
            {
                smith.isStopped = true; //내비게이션 에이전트의 이동을 멈추로 경로를 초기화한다.
                smith.ResetPath();
                smith.stoppingDistance = attackDistance; // 내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다.
                smith.destination = player.position; //내비게이션의 목적지를 플레이어의 위치로 설정한다.
            }
        }
        else // 2m이내에 있다면.. 공격
        {
            if (PlayerMove.inReturnSpace == 0)
            {
                m_State = EnemyState.Attack;
                print("상태전환:Move->Attack");
                currentTime = attackDelay;
                anim.SetTrigger("MoveToAttackDelay");
            }
        }
    }
    void Attack()
    {
        //2m 이내라면
        if (Vector3.Distance(transform.position, player.position) < attackDistance)// && PlayerMove.inReturnSpace == 0)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay) // 2초에 한 번씩 공격
            {
            //    player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                SoundManager.instance.PlayZombieAttack();
                currentTime = 0;
                anim.SetTrigger("StartAttack");
            }
        }
        else //2m 밖이라면
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack->Move");
            currentTime = 0;
            anim.SetTrigger("AttackToMove");
        }
    }
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }
    void Return()
    {

        //초기 위치에서 거리가 0.1f 이상이라면 초기 위치 쪽으로 이동.
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            /*
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.forward = dir;
            */
            smith.destination = originPos; // 내비게이션의 목적지를 초기 저장된 위치로 설정함
            smith.stoppingDistance = 0; // 내비게이션으로 접근하는 최소 거리를 '0'으로 설정함
            print("1초기 위치에서 0.1f 이상");
        }
        //아니라면 자신의 위치를 초기 위치로 조정하고 현재 상태를 대기로 전환
        else
        {
            smith.isStopped = true; //내비게이션 에이전트의 이동을 멈추고 경로를 초기화 함.
            smith.ResetPath();
            //위치 값과 회전 값을 초기 상태로 변환한다.
            transform.position = originPos;
            transform.rotation = originRot;
            //hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");
            anim.SetTrigger("MoveToIdle");
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1.0f);

        m_State = EnemyState.Move;
        SoundManager.instance.PlayZombieDie();
        print("상태 전환 : Damaged -> Move");
    }
    public void HitEnemy(int hitPower)
    {
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)// || m_State == EnemyState.Return)
        {
            return;
        }

        hp -= hitPower; // 내비게이션 에이전트의 이동을 멈추고 경로를 초기화함.
        smith.isStopped = true;
        smith.ResetPath();
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any State -> Damaged");
            anim.SetTrigger("Damaged");
            Damaged();
        }
        else {
            m_State = EnemyState.Die;
            print("상태 전환 : Any State -> Die");
            anim.SetTrigger("Die");
            Die();
        }
    }
    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        if (gameObject.name.Contains("NORMAL"))
            zs.zb_Normal -= 1;
        else if (gameObject.name.Contains("RED"))
            zs.zb_Red -= 1;
        else if (gameObject.name.Contains("BLUE"))
            zs.zb_Blue -= 1;
        PlayerFire.killCnt += 1;
        // *******총 킬 수 갱신******* // 
        int a = PlayerPrefs.GetInt(LoginManager.LoginID + "Kill");  // 총 킬 수
        a += 1;
        PlayerPrefs.SetInt(LoginManager.LoginID + "Kill", a);  // ++1
        GameObject de = Instantiate(dieEff);
        de.transform.position = transform.position;
        SoundManager.instance.PlayZombieDie();
        cc.enabled = false;
        yield return new WaitForSeconds(2f);
        print("소멸");
        Destroy(gameObject);
    }
}
