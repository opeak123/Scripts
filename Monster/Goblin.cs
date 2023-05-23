using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Goblin : MonsterMovement
{
    private MonsterManager monsterManager;
    private MonsterData goblinData;
    private MonsterAnimation monsterAni;
    private NavMeshAgent navMesh;

    protected override bool isAttack { get; set; } = false;
    protected override bool isMoving { get; set; } = true;
    protected override bool isTrace { get; set; } = false;
    public bool trace;
    protected override float moveSpeed { get; set; }
    protected override Transform playerTransform { get; set; }

    //플레이어와의 거리 측정
    private Vector3 originPos;
    private float dirXZ;
    private float dirY;

    private void Awake()
    {
        originPos = this.transform.position;
        monsterAni = GetComponent<MonsterAnimation>();
        navMesh = GetComponent<NavMeshAgent>();
        monsterManager = FindObjectOfType<MonsterManager>();
        playerTransform = GameObject.Find("Player").transform;
    }
    private void Start()
    {
        if (goblinData == null)
        {
            goblinData = monsterManager.GetMonsterData(MonsterType.Goblin);
        }
        else if(goblinData != null)
        {
            Debug.Log("GolbinData가 이미 존재합니다.");
        }

        moveSpeed = monsterManager.GetMonsterData(MonsterType.Goblin).GetSPEED();
        navMesh.speed += moveSpeed;
    }

    private void Update()
    {
        Distance();
        Debug.Log(isTrace);
    }


    protected override void MoveToward()
    {
        navMesh.destination = playerTransform.position;
    }
    protected override void MoveToOrigin()
    {
        navMesh.destination = originPos;
    }

    private void Distance()
    {
        dirXZ = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
            new Vector2(playerTransform.position.x, playerTransform.position.z));

        dirY = Mathf.Abs(transform.position.y - playerTransform.position.y);

        if(dirXZ <= 10f && dirY <= 2f)
        {
            isTrace = true;
            MoveToward();
        }
        else
        {
            isTrace = false;
            MoveToOrigin();
        }
    }


    IEnumerator AttackAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        monsterAni.Attack();
    }

}