using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    int _hp;
    int _maxHp;
    int _fireDefense;
    int _waterDefense;
    int _lightningDefense;
    DefineHelper.ElementType _elementType;

    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }

    public int MaxHp
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }

    public int FireDefense
    {
        get { return _fireDefense; }
        set { _fireDefense = value; }
    }

    public int WaterDefense
    {
        get { return _waterDefense; }
        set { _waterDefense = value; }
    }
    public int LightningDefense
    {
        get { return _lightningDefense; }
        set { _lightningDefense = value; }
    }

    public DefineHelper.ElementType ElementType
    {
        get { return _elementType; }
        set { _elementType = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _hp = 100;
        _maxHp = 100;
        _fireDefense = 0;
        _waterDefense = 0;
        _lightningDefense = 0;
        _elementType = 0;
    }

    
}
