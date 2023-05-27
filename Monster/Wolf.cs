using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

[RequireComponent(typeof(MonsterAnimation))]
public class Wolf : MonsterMovement
{
    private MonsterManager monsterManager;
    //몬스터 데이터 값 
    private MonsterData wolfData;
    //몬스터애니메이션
    private MonsterAnimation monsterAni;
    //NaveMeshAgent
    private NavMeshAgent navMesh;
    protected override float moveSpeed { get; set; }
    protected override bool isMoving { get; set; } = false;
    protected override bool isTrace { get; set; } = false;
    protected override bool isAttack { get; set; } = false;
    protected override bool isDamaged { get; set; } = false;
    protected override bool isDead { get; set; } = false;
    protected override Transform playerTransform { get; set; }
    private Vector3 originPos;

    //몬스터 체력 
    private Slider slider;
    private float wolfCurrentHP = 0f;
    //전투상태 체크
    private float combatState = 0f;
    //몬스터 공격 쿨타임
    private float wolfAttackTimer = 0f;
    //몬스터 공격 범위
    private float attackRange = 4f;
    //몬스터 추적 범위
    private float traceRange = 20f;
    //다른 몬스터를 감지할 반지름
    private float detectionRadius = 10f;
    //반지름 내 Wolf 몬스터 수 
    private int monsterNum = 1;

    private void Awake()
    {
        //상속
        originPos = this.transform.position;
        slider = GetComponentInChildren<Slider>();
        monsterAni = GetComponent<MonsterAnimation>();
        navMesh = GetComponent<NavMeshAgent>();
        monsterManager = FindObjectOfType<MonsterManager>();
        //플레이어 위치
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        //데이터 확인
        if (wolfData == null)
        {
            wolfData = monsterManager.GetMonsterData(MonsterType.Wolf);
        }
        else if (wolfData != null)
        {
            Debug.Log("wolfData가 이미 존재합니다.");
        }
        wolfCurrentHP = wolfData.GetMaxHP();
        moveSpeed = monsterManager.GetMonsterData(MonsterType.Wolf).GetSPEED();
        navMesh.speed += moveSpeed;
    }

    private void Update()
    {
        if (isDead)
            return;

        WolfFarFromPlayer();
        DetectOtherMonster();
        WolfState();
        Debug.Log(monsterNum);
        var t = Random.Range(5, 50);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HitWolf(t);
        }
    }

    //플레이어 추적
    protected override void MoveToward()
    {
        navMesh.destination = playerTransform.position;
    }
    //몬스터가 원래자리로 되돌아감
    protected override void MoveToOrigin()
    {
        navMesh.destination = originPos;
    }

    //플레이어와 거리 계산
    private void WolfState()
    {
        float dir = Vector3.Distance(this.transform.position, playerTransform.position);
        isTrace = dir <= traceRange && !isAttack;
        isAttack = dir < attackRange;
        isMoving = navMesh.velocity.magnitude > 0.1f;
        wolfAttackTimer += Time.deltaTime;

        if (isDamaged)
            return;

        //애니메이션 재생 및 상황에 따른 몬스터 속도제어
        if (isTrace)
        {
            MoveToward();
            monsterAni.Run();
            moveSpeed = 20f;
            navMesh.acceleration = 100f;
            navMesh.angularSpeed = 300f;
            this.transform.rotation.SetLookRotation(playerTransform.transform.position);
        }
        else if(isAttack)
        {
            navMesh.destination = this.transform.position;
            if(wolfAttackTimer > 1f && !isTrace)
            {
                monsterAni.Attack();
                wolfAttackTimer = 0f;
            }
            else
            {
                monsterAni.Idle();
            }
        }
        else if(isMoving && !isTrace)
        {
            moveSpeed = 1f;
            navMesh.acceleration = 1f;
            navMesh.angularSpeed = 100f;
            monsterAni.Walk();
        }
        else
        {
            MoveToOrigin();
            monsterAni.Idle();
        }
    }
    //슬라이더에 최대체력 대입
    private void WolfHpSlider()
    {
        slider.maxValue = (float)wolfData.GetMaxHP();
        slider.value = wolfCurrentHP;
    }

    //호출 함수
    //울프가 데미지 받았을때
    private void HitWolf(int damage)
    {
        isDamaged = true;
        wolfCurrentHP -= damage;
        monsterAni.Damaged();

        WolfHpSlider();
        if (wolfCurrentHP <= 0)
        {
            isDead = true;
            MonsterDead();
        }
        if(combatState > 10)
        {
            isDamaged = false;
        }
    }
    //플레이어에게 피격시 반대방향으로 도망
    private void WolfFarFromPlayer()
    {
        if (isDamaged)
        {
            Vector3 farFromPlayer = transform.position + (transform.position - playerTransform.position).normalized;
            navMesh.SetDestination(farFromPlayer);
            monsterAni.Run();
            combatState += Time.deltaTime;
            if (combatState > 10f)
            {
                isDamaged = false;
                combatState = 0f;
            }
        }
    }
    //몬스터가 죽었을 경우
    protected override void MonsterDead()
    {
        navMesh.height = 0;
        navMesh.radius = 0;
        navMesh.acceleration = 0;
        monsterAni.Dead();
        Destroy(this.gameObject, 4f);
    }

    //반지름 내 주위 몬스터 감지
    private void DetectOtherMonster()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        monsterNum = 1;
        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("WOLF") && col.name != this.name)
            {
                monsterNum++;
            }
        }
        if (monsterNum != 1)
        {
            isDamaged = false;
        }
        else if (monsterNum == 1)
        {
            WolfFarFromPlayer();
        }
    }

    //반지름을 기즈모로 그림
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
