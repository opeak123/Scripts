using System.Collections.Generic;
using UnityEngine;

public enum MonsterType // 몬스터 타입
{
    Goblin,
    Wolf,
    Troll
}

public class MonsterManager : MonoBehaviour
{
    //인스턴스화
    public static MonsterManager Instance { get; set; }
    //Dictionary
    private Dictionary<MonsterType, MonsterData> monsterDic = new Dictionary<MonsterType, MonsterData>();
    
    private void Awake()
    {
        #region 싱글톤
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        #endregion
        monsterDic.Clear();
    }
    private void Start()
    {
        GoblinData();
        WolfData();
        TrollData();
    }

    private void GoblinData()
    {
        MonsterData goblinData = new MonsterData();

        goblinData.SetHP(100);
        goblinData.SetDEF(20);
        goblinData.SetATK(20);
        goblinData.SetSPEED(15);
        goblinData.SetSTUN(false);
        goblinData.SetBURN(false);
        goblinData.SetPROVOKE(false);

        monsterDic.Add(MonsterType.Goblin, goblinData);
    }
    private void WolfData()
    {
        MonsterData wolfData = new MonsterData();

        wolfData.SetHP(100);
        wolfData.SetDEF(10);
        wolfData.SetATK(10);
        wolfData.SetSPEED(20);
        wolfData.SetSTUN(false);
        wolfData.SetBURN(false);
        wolfData.SetPROVOKE(false);

        monsterDic.Add(MonsterType.Wolf, wolfData);
    }
    private void TrollData()
    {
        MonsterData trollData = new MonsterData();

        trollData.SetHP(150);
        trollData.SetDEF(50);
        trollData.SetATK(30);
        trollData.SetSPEED(10);
        trollData.SetSTUN(false);
        trollData.SetBURN(false);
        trollData.SetPROVOKE(false);

        monsterDic.Add(MonsterType.Troll, trollData);
    }



}
