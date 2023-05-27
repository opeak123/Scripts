using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(MonsterAnimation))]
public class Goblin : MonsterMovement
{
    //���͸Ŵ��� 
    private MonsterManager monsterManager;
    //���� ������ �� 
    private MonsterData goblinData;
    //���;ִϸ��̼�
    private MonsterAnimation monsterAni;
    //NaveMeshAgent
    private NavMeshAgent navMesh;
    //�߻� ���
    protected override bool isAttack { get; set; } = false;
    protected override bool isMoving { get; set; } = true;
    protected override bool isTrace { get; set; } = false;
    protected override bool isDamaged { get; set; } = false;
    protected override bool isDead { get; set; } = false;
    protected override float moveSpeed { get; set; }
    protected override Transform playerTransform { get; set; }

    //�÷��̾���� �Ÿ� ����
    private Vector3 originPos;
    private float dirXZ;
    private float dirY;

    //���� ������Ÿ��
    private float goblinAttackTimer;

    //���� ü��
    private Slider slider;
    private float goblinCurrentHP;

    //���� ü�� �ڵ�ȸ��
    private float recoveryHpTimer;
    private void Awake()
    {
        //�Ҵ�
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
        if (goblinData == null)
        {
            goblinData = monsterManager.GetMonsterData(MonsterType.Goblin);
        }
        else if(goblinData != null)
        {
            Debug.Log("GolbinData�� �̹� �����մϴ�.");
        }
        //������ �� ���� 
        goblinCurrentHP = goblinData.GetMaxHP();
        moveSpeed = monsterManager.GetMonsterData(MonsterType.Goblin).GetSPEED();
        navMesh.speed += moveSpeed;
    }
    private void Update()
    {
        if (this.isDead)
            return;
        //���Ͱ� �÷��̾��� �����̼��� �ٶ󺸰� ��
        this.transform.rotation.SetLookRotation(playerTransform.transform.position);
        //�÷��̾���� �Ÿ����
        GoblinState();

        //var t = Random.Range(5, 50);
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    HitGoblin(t);
        //    Debug.Log(this.gameObject.name + "HP:" + goblinCurrentHP);
        //}
    }
    //���Ϳ� �÷��̾��� �Ÿ� �����ؼ� ��ã��
    protected override void MoveToward()
    {
        navMesh.destination = playerTransform.position;
    }
    //���Ͱ� �����ڸ��� �ǵ��ư�
    protected override void MoveToOrigin()
    {
        navMesh.destination = originPos;
    }
    
    private void GoblinState()
    {
        //�÷��̾�� ������ �������� X,Z�� ���
        dirXZ = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
            new Vector2(playerTransform.position.x, playerTransform.position.z));

        //�÷��̾�� ������ �������� Y�� ���
        dirY = Mathf.Abs(transform.position.y - playerTransform.position.y);
        //���� ���� ��Ÿ��
        goblinAttackTimer += Time.deltaTime;
        //Bool ���� : ����/����/�̵�������
        isTrace = dirXZ <= 20f && dirY <= 2f && !isAttack;
        isAttack = dirXZ < 3f;
        isMoving = navMesh.velocity.magnitude > 0.1f;

        //�ִϸ��̼� ���
        switch (isTrace)
        {
            case true:
                MoveToward();
                monsterAni.Run();
                break;

            case false:
                if(isMoving)
                {
                    monsterAni.Walk();
                }
                break;
        }

        //�ִϸ��̼� ���
        switch (isAttack)
        {
            case true:
                if (isAttack)
                {
                    navMesh.destination = this.transform.position;
                    if (goblinAttackTimer >= 3 && !isTrace && !isMoving)
                    {
                        monsterAni.Attack();
                        goblinAttackTimer = 0f;
                    }
                    else
                    {
                        monsterAni.Idle();
                    }
                }
                break;
        }
        //�ִϸ��̼� ���
        if (isMoving)
        {
            monsterAni.Walk();
        }
        else if(!isMoving && !isTrace && !isAttack)
        {
            MoveToOrigin();
            monsterAni.Idle();
            RecoveryHP();
        }
    }

    //������ ���������� �����̴��� �ִ�ü�� ����
    private void GoblinHpSlider()
    {
        slider.maxValue = (float)goblinData.GetMaxHP();
        slider.value = goblinCurrentHP;
    }

    //������� �������� ������ ��
    private void HitGoblin(int damage)
    {
        goblinCurrentHP -= damage;
        monsterAni.Damaged();

        GoblinHpSlider();
        if (goblinCurrentHP <= 0)
        {
            isDead = true;
            MonsterDead();
        }
    }

    //����� �׾����� State
    protected override void MonsterDead()
    {
        navMesh.height = 0;
        navMesh.radius = 0;
        monsterAni.Dead();
        Destroy(this.gameObject, 4f);
    }

    //�������°� �ƴ϶�� ü�� �ڵ�ȸ��
    private void RecoveryHP()
    {
        recoveryHpTimer += Time.deltaTime;
        float amount = goblinData.GetMaxHP() / 99f;
        if (recoveryHpTimer > 0.5f)
        {
            slider.value += amount;
            recoveryHpTimer = 0f;
        }
    }

}