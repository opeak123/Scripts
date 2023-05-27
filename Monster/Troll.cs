using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(MonsterAnimation))]
public class Troll : MonsterMovement
{
    private MonsterManager monsterManager;
    //몬스터 데이터 값 
    private MonsterData trollData;
    //몬스터애니메이션
    private MonsterAnimation monsterAni;
    //NaveMeshAgent
    private NavMeshAgent navMesh;

    protected override float moveSpeed { get; set; }
    protected override bool isMoving { get; set; }
    protected override bool isTrace { get; set; }
    protected override bool isAttack { get; set; }
    protected override bool isDamaged { get; set; }
    protected override bool isDead { get; set; }
    protected override Transform playerTransform { get; set; }
    private Vector3 originPos;

    //몬스터 체력 
    private Slider slider;
    private float trollCurrentHP = 0f;

    //몬스터 공격 쿨타임
    private float trollAttackTimer = 0f;
    //몬스터 공격 범위
    private float attackRange = 4f;
    //몬스터 추적 범위
    private float traceRange = 20f;

    //콜라이더에 닿으면 플레이어의 공격력과 방어력을 깎음
    private SphereCollider sphereCollider;
    //private bool debuffapplied = false;
    private int debuffActiveNum = 0;
    private int debuffAmount = 0;

    private void Awake()
    {
        //상속
        originPos = this.transform.position;
        slider = GetComponentInChildren<Slider>();
        monsterAni = GetComponent<MonsterAnimation>();
        navMesh = GetComponent<NavMeshAgent>();
        monsterManager = FindObjectOfType<MonsterManager>();
        sphereCollider = transform.GetChild(0).GetComponent<SphereCollider>();
        //플레이어 위치
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        //데이터 Null 검사
        if (trollData == null)
        {
            trollData = monsterManager.GetMonsterData(MonsterType.Troll);
        }
        else if (trollData != null)
        {
            Debug.Log("trollData가 이미 존재합니다.");
        }
        trollCurrentHP = trollData.GetMaxHP();
        moveSpeed = monsterManager.GetMonsterData(MonsterType.Troll).GetSPEED();
        navMesh.speed += moveSpeed;
    }

    void Update()
    {
        TrollState();
        Debug.Log(trollData.GetDEF());
    }

    public void TrollState()
    {
        float dir = Vector3.Distance(this.transform.position, playerTransform.position);
        isTrace = dir <= traceRange && !isAttack;
        isAttack = dir < attackRange;
        isMoving = navMesh.velocity.magnitude > 0.1f;
        trollAttackTimer += Time.deltaTime;

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
        else if (isAttack)
        {
            navMesh.destination = this.transform.position;
            if (trollAttackTimer > 1f && !isTrace)
            {
                monsterAni.Attack();
                trollAttackTimer = 0f;
            }
            else
            {
                monsterAni.Idle();
            }
        }
        else if (isMoving && !isTrace)
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

    //플레이어를 찾아서 추적
    protected override void MoveToward()
    {
        navMesh.destination = playerTransform.position;
    }
    
    //제자리로 되돌아감
    protected override void MoveToOrigin()
    {
        navMesh.destination = originPos;
    }

    //몬스터가 죽었을 때
    protected override void MonsterDead()
    {
        isDead = true;
    }


    //몬스터의 sphereCollider에 머물러있으면 플레이어 20%만큼 디버프(중첩)
    private void OnTriggerStay(Collider col)
    {
        if(debuffActiveNum == 0)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                debuffActiveNum++;
                //debuffapplied = true;
                PlayerDebuff();
            }
        }
    }
    //sphereCollider에 나갔을 때 디버프 제거 
    private void OnTriggerExit(Collider col)
    {
        if(debuffActiveNum > 0)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                //debuffapplied = false;
                PlayerRemoveDebuff();
                debuffActiveNum--;
            }
        }
    }

    //플레이어 디버프
    private void PlayerDebuff() // 플레이어 데이터 정보로 변경해야 합니다
    {
        int debuff = Mathf.RoundToInt(trollData.GetDEF() * 0.2f);
        debuffAmount += debuff;
        int reducedDEF = trollData.GetDEF() - debuffAmount;
        trollData.SetDEF(reducedDEF);
    }
    //플레이어 디버프 제거
    private void PlayerRemoveDebuff()// 플레이어 데이터 정보로 변경해야 합니다
    {
        int originalDEF = trollData.GetDEF() + debuffAmount;
        trollData.SetDEF(originalDEF);
        debuffAmount = 0;
    }
}
