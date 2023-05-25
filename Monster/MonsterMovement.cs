using UnityEngine;

//������ ������ �� �߻�Ŭ���� 
public abstract class MonsterMovement : MonoBehaviour
{
    protected abstract float moveSpeed { get; set; }
    protected abstract bool isMoving { get; set; }
    protected abstract bool isTrace { get; set; }
    protected abstract bool isAttack { get; set; }
    protected abstract bool isDamaged { get; set; }
    protected abstract bool isDead { get; set; }
    protected abstract Transform playerTransform { get; set; }
    protected abstract void MoveToward();
    protected abstract void MoveToOrigin();
    protected abstract void MonsterDead();
}
