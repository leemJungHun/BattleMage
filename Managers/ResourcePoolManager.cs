using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolManager : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] _playerType;
    [SerializeField] RuntimeAnimatorController[] _monsterType;
    [SerializeField] Sprite[] _heartSprite;
    [SerializeField] Sprite[] _attackSkillSprites;
    [SerializeField] GameObject[] _windowPrefabs;
    [SerializeField] AudioClip[] _BgmList;
    [SerializeField] AudioClip[] _FxList;

    static ResourcePoolManager _unique;

    public static ResourcePoolManager instance
    {
        get { return _unique; }
    }
    private void Awake()
    {
        _unique = this;
    }

    public RuntimeAnimatorController GetPlayerType(DefineHelper.ElementType elementType)
    {
        return _playerType[(int)elementType];
    }

    public RuntimeAnimatorController GetMonsterType(DefineHelper.ElementType elementType)
    {
        return _monsterType[(int)elementType];
    }

    public Sprite GetHeartSprite(bool isOn)
    {
        return isOn ? _heartSprite[0] : _heartSprite[1];
    }

    public Sprite GetAttackIconSprite(DefineHelper.ElementType elementType)
    {
        return _attackSkillSprites[(int)elementType];
    }

    public AudioClip GetBgmClipFormType(DefineHelper.eBgmType type)
    {
        return _BgmList[(int)type];
    }

    public AudioClip GetFxClipFormType(DefineHelper.eFxType type)
    {
        return _FxList[(int)type];
    }

    public GameObject GetWindowPrefabFromType(DefineHelper.eUIwindowtype windowtype)
    {
        return _windowPrefabs[(int)windowtype];
    }
}
