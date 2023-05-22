public class MonsterData
{
    private int HP { get; set; }    // ü��
    private int DEF { get; set; }  // ����
    private int ATK { get; set; }  // ���ݷ�
    private float SPEED { get; set; } // �ӵ�

    private bool STUN { get; set; } // ����
    private bool BURN { get; set; } // ȭ��
    private bool PROVOKE { get; set; } // ����

    // Getter �޼���
    public int GetHP() => HP;
    public int GetDEF() => DEF;
    public int GetATK() => ATK;
    public float GetSPEED() => SPEED;
    public bool GetSTUN() => STUN;
    public bool GetBURN() => BURN;
    public bool GetPROVOKE() => PROVOKE;

    // Setter �޼���
    public void SetHP(int value) => HP = value;
    public void SetDEF(int value) => DEF = value;
    public void SetATK(int value) => ATK = value;
    public void SetSPEED(float value) => SPEED = value;
    public void SetSTUN(bool value) => STUN = value;
    public void SetBURN(bool value) => BURN = value;
    public void SetPROVOKE(bool value) => PROVOKE = value;
}