using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(MonsterAnimation))]
public class Wolf : MonsterMovement
{
    private MonsterManager monsterManager;
    //���� ������ �� 
    private MonsterData wolfData;
    //���;ִϸ��̼�
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

    //���� ü�� 
    private Slider slider;
    private float wolfCurrentHP = 0f;
    //�������� üũ
    private float combatState = 0f;
    //���� ���� ��Ÿ��
    private float wolfAttackTimer = 0f;
    //�ٸ� ���͸� ������ ������
    private float detectionRadius = 10f;
    //������ �� Wolf ���� �� 
    private int monsterNum = 1;

    private void Awake()
    {
        //���
        originPos = this.transform.position;
        slider = GetComponentInChildren<Slider>();
        monsterAni = GetComponent<MonsterAnimation>();
        navMesh = GetComponent<NavMeshAgent>();
        monsterManager = FindObjectOfType<MonsterManager>();
        //�÷��̾� ��ġ
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        //������ Ȯ��
        if (wolfData == null)
        {
            wolfData = monsterManager.GetMonsterData(MonsterType.Wolf);
        }
        else if (wolfData != null)
        {
            Debug.Log("wolfData�� �̹� �����մϴ�.");
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

    //�÷��̾� ����
    protected override void MoveToward()
    {
        navMesh.destination = playerTransform.position;
    }
    //���Ͱ� �����ڸ��� �ǵ��ư�
    protected override void MoveToOrigin()
    {
        navMesh.destination = originPos;
    }

    //�÷��̾�� �Ÿ� ���
    private void WolfState()
    {
        float dir = Vector3.Distance(this.transform.position, playerTransform.position);
        isTrace = dir <= 20f && !isAttack;
        isAttack = dir < 4f;
        isMoving = navMesh.velocity.magnitude > 0.1f;
        wolfAttackTimer += Time.deltaTime;

        

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
    //�����̴��� �ִ�ü�� ����
    private void WolfHpSlider()
    {
        slider.maxValue = (float)wolfData.GetMaxHP();
        slider.value = wolfCurrentHP;
    }

    //ȣ�� �Լ�
    //������ ������ �޾�����
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
    //�÷��̾�� �ǰݽ� �ݴ�������� ����
    private void WolfFarFromPlayer()
    {
        if (isDamaged && this.monsterNum > 1)
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
    //���Ͱ� �׾��� ���
    protected override void MonsterDead()
    {
        navMesh.height = 0;
        navMesh.radius = 0;
        monsterAni.Dead();
        Destroy(this.gameObject, 4f);
    }

    //������ �� ���� ���� ����
    private void DetectOtherMonster()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        monsterNum = 1;
        foreach(Collider col in colliders)
        {
            if(col.gameObject.CompareTag("WOLF") && col.name != this.name)
            {
                monsterNum++;
            }
        }
    }

    //�������� ������ �׸�
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
