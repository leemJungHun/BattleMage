using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCotroller : MonoBehaviour
{
    Animator _ani;
    MonsterStat _myStat;
    DefineHelper.ElementType _monsterType;
    CapsuleCollider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        _ani = GetComponent<Animator>();
        _myStat = GetComponent<MonsterStat>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TypeSet(DefineHelper.ElementType type)
    {
        _monsterType = type;
        _myStat.InitStat(type);
        _collider.enabled = true;
        _ani.SetBool("isDead", false);
    }
    public void ColliderOff()
    {
        if (_myStat.HP <= 0)
        {
            _collider.enabled = false;
        }
    }
    public void ActiveFlase()
    {
        if(_myStat.HP <= 0)
        {
            _ani.SetBool("isDead", false);
            gameObject.SetActive(false);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != null)
        {
            if (collision.gameObject.CompareTag("Attack")|| collision.gameObject.CompareTag("DashSkill"))
            {
                if (_myStat.HP > 0)
                {
                    bool isDash = false;
                    PlayerController._instance.Hit();
                    int damage = 100;
                    switch (PlayerController._instance.PlayerType)
                    {
                        case DefineHelper.ElementType.Fire:
                            damage -= _myStat.FireDefense;
                            break;
                        case DefineHelper.ElementType.Water:
                            damage -= _myStat.WaterDefense;
                            break;
                        case DefineHelper.ElementType.Lightning:
                            damage -= _myStat.LightningDefense;
                            break;
                    }

                    if (collision.gameObject.CompareTag("DashSkill"))
                    {
                        damage = 100;
                        isDash = true;
                    }

                    _myStat.HP -= damage;

                    InGameManager._instance.Combo(true);

                    if (_myStat.HP <= 0 || PlayerController._instance.IsHurt)
                    {
                        SoundManager.instance.PlaySfxSoundOneShot(DefineHelper.eFxType.Dead);
                        _myStat.HP = 0;
                        _ani.SetBool("isDead", true);
                        DropMonster._instance.MonsterDie(isDash);
                    }
                    else
                    {
                        SoundManager.instance.PlaySfxSoundOneShot(DefineHelper.eFxType.Hit);
                        _ani.SetTrigger("Hit");
                    }
                }
            }

            if (collision.gameObject.CompareTag("Barrier"))
            {
                SoundManager.instance.PlaySfxSoundOneShot(DefineHelper.eFxType.Defense);
                if (PlayerController._instance.IsGround)
                {
                    InGameManager._instance.Combo(false);
                }
                DropMonster._instance.Defense();
            }

            if (collision.gameObject.CompareTag("BarrierSkill"))
            {
                SoundManager.instance.PlaySfxSoundOneShot(DefineHelper.eFxType.Defense);
                DropMonster._instance.BarrierSkill();
            }

            if (collision.gameObject.CompareTag("Hurt"))
            {
                InGameManager._instance.Combo(false);
                StartCoroutine(InGameManager._instance.PlayerDamage());
            }
        }
    }

    
}
