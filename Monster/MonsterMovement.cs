using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterMovement : MonoBehaviour
{
    protected abstract float moveSpeed { get; set; }
    protected abstract bool isMoving { get; set; }
    protected abstract bool isTrace { get; set; }
    protected abstract bool isAttack { get; set; }

    protected abstract Transform playerTransform { get; set; }
    protected abstract void MoveToward();
    protected abstract void MoveToOrigin();
}
