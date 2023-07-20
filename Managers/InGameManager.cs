using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameManager : MonoBehaviour
{
    static InGameManager _uniqueInstance;
    [SerializeField] GameObject _dropMonster;
    [SerializeField] Transform _regenPos;
    [SerializeField] GameObject[] _mosters;
    [SerializeField] Slider _stageBar;
    [SerializeField] Button[] _changeButton;
    [SerializeField] Image[] _changeImage;
    [SerializeField] Image _damagerBG;
    [SerializeField] Image[] _heart;
    [SerializeField] Text _comboText;
    // 정보 변수
    DefineHelper.eIngameState _currentState;
    int _currentWave = 1;
    int _waveCount = 0;
    int _stageHeart = 3;
    int _combo = 0;
    int _maxCombo = 0;
    float _comboTime = 3f;
    public static InGameManager _instance
    {
        get { return _uniqueInstance; }
    }

    public int StageHeart
    {
        get { return _stageHeart; }
    }

    public int MaxCombo
    {
        get { return _maxCombo; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeSettings(DefineHelper.ElementType.Fire);
    }

    public void InitializeSettings(DefineHelper.ElementType playerType)
    {
        _currentWave = 1;
        _waveCount = 0;
        PlayerController._instance.PlayerType = playerType;
        _currentState = DefineHelper.eIngameState.PLAY;
        ChangeMage((int)playerType);
        NextWave();
    }

    private void Update()
    {
        if(_currentState == DefineHelper.eIngameState.PLAY)
        {
            _comboTime -= Time.deltaTime;
            if(_comboTime <= 0f)
            {
                _combo = 0;
                _comboText.text = string.Empty;
            }
        }
    }

    public void NextWave()
    {
        _dropMonster.transform.position = _regenPos.position;
        int randomMonster = Random.Range(6, 10); // 몬스터 마리수 랜덤
        DefineHelper.ElementType monsterType = DefineHelper.ElementType.Fire;
        switch (_currentWave)
        {
            case 1:
                switch (_waveCount)
                {
                    case 0:
                        monsterType = DefineHelper.ElementType.Fire;
                        _waveCount++;
                        break;
                    case 1:
                        monsterType = DefineHelper.ElementType.Water;
                        _waveCount = 0;
                        _stageBar.value = 0.125f;
                        _currentWave++;
                        break;
                }
                break;
            case 2:
                switch (_waveCount)
                {
                    case 0:
                        monsterType = DefineHelper.ElementType.Water;
                        _waveCount++;
                        _stageBar.value = 0.25f;
                        break;
                    case 1:
                        monsterType = DefineHelper.ElementType.Lightning;
                        _waveCount = 0;
                        _stageBar.value = 0.375f;
                        _currentWave++;
                        break;
                }
                break;
            case 3:
                switch (_waveCount)
                {
                    case 0:
                        monsterType = DefineHelper.ElementType.Lightning;
                        _stageBar.value = 0.5f;
                        _waveCount++;
                        break;
                    case 1:
                        monsterType = DefineHelper.ElementType.Fire;
                        _stageBar.value = 0.625f;
                        _waveCount = 0;
                        _currentWave++;
                        break;
                }
                break;
            case 4:
                switch (_waveCount)
                {
                    case 0:
                        monsterType = DefineHelper.ElementType.Fire;
                        _stageBar.value = 0.75f;
                        _waveCount++;
                        break;
                    case 1:
                        monsterType = DefineHelper.ElementType.Water;
                        _stageBar.value = 0.83f;
                        _waveCount++;
                        break;
                    case 2:
                        monsterType = DefineHelper.ElementType.Lightning;
                        _stageBar.value = 0.91f;
                        _waveCount = 0;
                        _currentWave++;
                        break;
                }
                break;
            case 5:
                _stageBar.value = 1f;
                EndGame(true);
                break;
        }

        if(_currentState == DefineHelper.eIngameState.PLAY)
        {
            for(int i = 0; i < _changeButton.Length; i++)
            {
                _changeButton[i].enabled = true;
                Color color = _changeImage[i].color;
                color.a = 1f;
                _changeImage[i].color = color;
            }


            RuntimeAnimatorController aniCtrl = ResourcePoolManager.instance.GetMonsterType(monsterType);

            for (int i = 0; i < _mosters.Length; i++)
            {
                if (i < randomMonster)
                {
                    _mosters[i].SetActive(true);
                    _mosters[i].GetComponent<Animator>().runtimeAnimatorController = aniCtrl;
                    _mosters[i].GetComponent<MonsterCotroller>().TypeSet(monsterType);
                }
                else
                {
                    _mosters[i].SetActive(false);
                }
            }

            DropMonster._instance.IsDrop(randomMonster);
        }
        
    }
    

    public void EndGame(bool isClear)
    {
        _currentState = DefineHelper.eIngameState.END;
        // 종료창을 생성
        GameObject go = Instantiate(ResourcePoolManager.instance.GetWindowPrefabFromType(DefineHelper.eUIwindowtype.ResultWnd));
        ResultWnd resultWnd = go.GetComponent<ResultWnd>();
        resultWnd.OpenResultWindow(_maxCombo, isClear);
    }

    public void ChangeMage(int type)
    {
        for (int i = 0; i < _changeButton.Length; i++)
        {
            _changeButton[i].enabled = false;
            Color color = _changeImage[i].color;
            color.a = 0.5f;
            _changeImage[i].color = color;
        }

        PlayerController._instance.ChangeMage((DefineHelper.ElementType)type);
    }

    public void Combo(bool isCombo)
    {
        if(_combo >= _maxCombo)
        {
            _maxCombo = _combo;
        }
        if (isCombo)
        {
            _combo++;
            _comboText.text = _combo.ToString() + "Combo";
            _comboTime = 3f;
        }
        else
        {
            _combo = 0;
            _comboText.text = string.Empty;
        }
    }

    public IEnumerator PlayerDamage()
    {
        _stageHeart--;
        if(_stageHeart >= 0)
        {
            PlayerController._instance.IsHurt = true;
            _damagerBG.enabled = true;
            yield return new WaitForSeconds(0.1f);
            _heart[_stageHeart].sprite = ResourcePoolManager.instance.GetHeartSprite(false);
            _damagerBG.enabled = false;
            if (_stageHeart <= 0)
            {
                EndGame(false);
            }
        }
        
    }
}
