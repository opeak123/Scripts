using UnityEngine;
public class MonsterAnimation : MonoBehaviour
{
    //���� �ִϸ��̼�
    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }
    
    //idle
    public void Idle()
    {
        ani.SetTrigger("idle");
    }
    //�ȱ�
    public void Walk()
    {
        ani.SetTrigger("walk");
    }
    //���Ͱ� ����
    public void Run()
    {
        ani.SetTrigger("run");
    }
    //����
    public void Attack()
    {
        ani.SetTrigger("attack");
    }
    //���Ͱ� ���ݹ޾��� ��
    public void Damaged()
    {
        ani.SetTrigger("damaged");
    }
    public void Dead()
    {
        ani.SetTrigger("dead");
    }
}