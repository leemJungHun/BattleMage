using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    static PlayerController _uniqueInstance;
    [SerializeField] float _jumpP;
    [SerializeField] float _downP;
    [SerializeField] GameObject _player;
    [SerializeField] Image _jumpSkill;
    [SerializeField] Image _attackSkill;
    [SerializeField] Image _barrierSkill;
    [SerializeField] GameObject _jumpSkillBar;
    [SerializeField] GameObject _attakSkillBar;
    [SerializeField] GameObject _barrierSkillBar;
    [SerializeField] Image _barrierCoolTime;
    [SerializeField] Image _skillBG;
    Rigidbody2D _rigid;
    BoxCollider2D _boxCol;
    CapsuleCollider2D _capCol;
    bool _isGround;
    Animator _ani;
    Scrollbar _jumpBar;
    Image _jumpBarBG;
    Image _jumpArrow;

    Scrollbar _attackBar;
    Image _attackBarBG;
    Image _attackArrow;


    Scrollbar _barrierBar;
    Image _barrierBarBG;
    Image _barrierArrow;
    DefineHelper.ElementType _playerType;
    bool _onBarrier = true;
    float _coolTime = 1f;
    bool _onAttackSkill = false;
    public DefineHelper.ElementType PlayerType
    {
        get { return _playerType; }
        set { _playerType = value; }
    }
    public bool IsGround
    {
        get { return _isGround; }
    }

    public bool IsHurt
    {
        get; set;
    }

    public static PlayerController _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        _rigid = GetComponent<Rigidbody2D>();
        _boxCol = GetComponent<BoxCollider2D>();
        _capCol = GetComponent<CapsuleCollider2D>();
        _jumpBar = _jumpSkillBar.GetComponent<Scrollbar>();
        _jumpBarBG = _jumpSkillBar.GetComponent<Image>();
        _jumpArrow = _jumpSkillBar.transform.GetChild(0).GetComponent<Image>();
        _attackBar = _attakSkillBar.GetComponent<Scrollbar>();
        _attackBarBG = _attakSkillBar.GetComponent<Image>();
        _attackArrow = _attakSkillBar.transform.GetChild(0).GetComponent<Image>();
        _barrierBar = _barrierSkillBar.GetComponent<Scrollbar>();
        _barrierBarBG = _barrierSkillBar.GetComponent<Image>();
        _barrierArrow = _barrierSkillBar.transform.GetChild(0).GetComponent<Image>();
    }

    void Start()
    {
        _ani = _player.GetComponent<Animator>();
    }

    private void Update()
    {

        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCol.bounds.center, _boxCol.bounds.size, 0f, Vector2.down, 0.02f, LayerMask.GetMask("Ground"));
        if(raycastHit.collider != null)
        {
            _isGround = true;
            _capCol.isTrigger = true;
            _ani.SetBool("IsGround", true);
        }
        else
        {
            _isGround = false;
            _capCol.isTrigger = false;
            _ani.SetBool("IsGround", false);
        }

        if (!_onBarrier)
        {
            _coolTime -= Time.deltaTime;
            if(_coolTime <= 0f)
            {
                _coolTime = 0f;
                _onBarrier = true;
            }
            _barrierCoolTime.fillAmount = _coolTime;
        }
    }

    public void PlayerJump()
    {
        if (_isGround && !IsHurt)
        {
            _rigid.AddForce(Vector2.up * _jumpP, ForceMode2D.Impulse);
            _jumpSkill.fillAmount += 0.34f;
            if(_jumpSkill.fillAmount >= 1)
            {
                _jumpBar.enabled = true;
                _jumpBarBG.enabled = true;
                _jumpArrow.enabled = true;
            }
        }
    }
    
    public void Attack()
    {
        StartCoroutine(AttackCorutine());
    }

    IEnumerator AttackCorutine()
    {
        _ani.SetTrigger("OnAttack");
        yield return new WaitForSeconds(0.1f);
        IsHurt = false;
    }

    public void AttackSkill()
    {
        if (_attackBar.value < 0.90f)
        {
            _attackBar.value = 0f;
            return;
        }

        StartCoroutine(AttackSkillBuff());
        _attackSkill.fillAmount = 0f;
        _attackBar.value = 0f;
        _attackBar.enabled = false;
        _attackBarBG.enabled = false;
        _attackArrow.enabled = false;
    }

    IEnumerator AttackSkillBuff()
    {
        _ani.SetBool("AttackSkill", true);
        _onAttackSkill = true;
        _skillBG.enabled = true;
        yield return new WaitForSeconds(10f);
        _ani.SetBool("AttackSkill", false);
        _onAttackSkill = false;
        _skillBG.enabled = false;
    }

    public void Barrier()
    {
        if (_onBarrier)
        {
            _ani.SetTrigger("OnBarrier");
            IsHurt = false;
            _onBarrier = false;
            _coolTime = 1f;
        }
    }
    

    public void JumpSkill()
    {
        Debug.Log("jumpSkill: " + _jumpBar.value);
        if(_jumpBar.value < 0.90f)
        {
            _jumpBar.value = 0f;
            return;
        }
        _ani.SetBool("JumpSkill", true);

        _jumpSkill.fillAmount = 0f;
        _jumpBar.value = 0f;
        _jumpBar.enabled = false;
        _jumpBarBG.enabled = false;
        _jumpArrow.enabled = false;
    }

    public void JumpBooster()
    {
        Debug.Log("dash Booster");
        _skillBG.enabled = true;
        _rigid.AddForce(Vector2.up * 50f, ForceMode2D.Impulse);
    }

    public void BarrierSkill()
    {
        Debug.Log("BarrierSkill: " + _barrierBar.value);
        if (_barrierBar.value < 0.90f)
        {
            _barrierBar.value = 0f;
            return;
        }
        _ani.SetTrigger("BarrierSkill");

        _skillBG.enabled = true;
        _barrierSkill.fillAmount = 0f;
        _barrierBar.value = 0f;
        _barrierBar.enabled = false;
        _barrierBarBG.enabled = false;
        _barrierArrow.enabled = false;

    }

    public void SkillStop()
    {
        _ani.SetBool("JumpSkill", false);
        if (!_onAttackSkill)
        {
            _skillBG.enabled = false;
        }
        _rigid.velocity = Vector2.zero;
        Debug.Log("dash Stop");
    }

    public void Hit()
    {
        if (_onAttackSkill)
        {
            return;
        }
        _attackSkill.fillAmount += 0.034f;
        if (_attackSkill.fillAmount >= 1)
        {
            _attackBar.enabled = true;
            _attackBarBG.enabled = true;
            _attackArrow.enabled = true;
        }
    }

    public void DefenseSuccess()
    {
        _barrierSkill.fillAmount += 0.20f;
        if (_barrierSkill.fillAmount >= 1)
        {
            _barrierBar.enabled = true;
            _barrierBarBG.enabled = true;
            _barrierArrow.enabled = true;
        }
        _rigid.velocity = Vector2.zero;
        _rigid.AddForce(Vector2.down * _downP, ForceMode2D.Impulse);
    }

    public void BarrierSkillEnd()
    {
        if (!_onAttackSkill)
        {
            _skillBG.enabled = false;
        }
    }

    public void ChangeMage(DefineHelper.ElementType type)
    {
        PlayerType = type;
        RuntimeAnimatorController aniCtrl = ResourcePoolManager.instance.GetPlayerType(type);
        _attackSkill.sprite = ResourcePoolManager.instance.GetAttackIconSprite(type);
        _ani.runtimeAnimatorController = aniCtrl;
    }

}
