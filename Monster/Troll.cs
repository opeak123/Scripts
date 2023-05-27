using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(MonsterAnimation))]
public class Troll : MonsterMovement
{
    private MonsterManager monsterManager;
    //���� ������ �� 
    private MonsterData trollData;
    //���;ִϸ��̼�
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

    //���� ü�� 
    private Slider slider;
    private float trollCurrentHP = 0f;

    //���� ���� ��Ÿ��
    private float trollAttackTimer = 0f;
    //���� ���� ����
    private float attackRange = 4f;
    //���� ���� ����
    private float traceRange = 20f;

    //�ݶ��̴��� ������ �÷��̾��� ���ݷ°� ������ ����
    private SphereCollider sphereCollider;
    //private bool debuffapplied = false;
    private int debuffActiveNum = 0;
    private int debuffAmount = 0;

    private void Awake()
    {
        //���
        originPos = this.transform.position;
        slider = GetComponentInChildren<Slider>();
        monsterAni = GetComponent<MonsterAnimation>();
        navMesh = GetComponent<NavMeshAgent>();
        monsterManager = FindObjectOfType<MonsterManager>();
        sphereCollider = transform.GetChild(0).GetComponent<SphereCollider>();
        //�÷��̾� ��ġ
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        //������ Null �˻�
        if (trollData == null)
        {
            trollData = monsterManager.GetMonsterData(MonsterType.Troll);
        }
        else if (trollData != null)
        {
            Debug.Log("trollData�� �̹� �����մϴ�.");
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

        //�ִϸ��̼� ��� �� ��Ȳ�� ���� ���� �ӵ�����
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

    //�÷��̾ ã�Ƽ� ����
    protected override void MoveToward()
    {
        navMesh.destination = playerTransform.position;
    }
    
    //���ڸ��� �ǵ��ư�
    protected override void MoveToOrigin()
    {
        navMesh.destination = originPos;
    }

    //���Ͱ� �׾��� ��
    protected override void MonsterDead()
    {
        isDead = true;
    }


    //������ sphereCollider�� �ӹ��������� �÷��̾� 20%��ŭ �����(��ø)
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
    //sphereCollider�� ������ �� ����� ���� 
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

    //�÷��̾� �����
    private void PlayerDebuff() // �÷��̾� ������ ������ �����ؾ� �մϴ�
    {
        int debuff = Mathf.RoundToInt(trollData.GetDEF() * 0.2f);
        debuffAmount += debuff;
        int reducedDEF = trollData.GetDEF() - debuffAmount;
        trollData.SetDEF(reducedDEF);
    }
    //�÷��̾� ����� ����
    private void PlayerRemoveDebuff()// �÷��̾� ������ ������ �����ؾ� �մϴ�
    {
        int originalDEF = trollData.GetDEF() + debuffAmount;
        trollData.SetDEF(originalDEF);
        debuffAmount = 0;
    }
}
