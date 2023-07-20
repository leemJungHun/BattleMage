using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMonster : MonoBehaviour
{
    static DropMonster _uniqueInstance;
    Rigidbody2D _rigid;
    bool _isDrop = false;
    int _dropMonsterCnt = 0;
    public static DropMonster _instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
    }

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_isDrop)
        {
            _rigid.AddForce(Vector3.down * 50f);
        }
    }

    public void IsDrop(int monsterCnt)
    {
        _isDrop = true;
        _dropMonsterCnt = monsterCnt;
    }

    public void MonsterDie(bool isDash)
    {
        _dropMonsterCnt--;
        if (_dropMonsterCnt == 0)
        {
            Debug.Log("남은 몬스터" + _dropMonsterCnt);
            if (isDash)
                PlayerController._instance.SkillStop();
            _isDrop = false;
            _rigid.velocity = Vector2.zero;
            InGameManager._instance.NextWave();
        }
    }

    public void Defense()
    {
        PlayerController._instance.IsHurt = false;
        _rigid.velocity = Vector2.zero;
        _rigid.AddForce(Vector3.up * 1500f);
        PlayerController._instance.DefenseSuccess();
    }

    public void BarrierSkill()
    {
        PlayerController._instance.IsHurt = false;
        _rigid.velocity = Vector2.zero;
        _rigid.AddForce(Vector3.up * 5000f);
        PlayerController._instance.BarrierSkillEnd();
    }
}
