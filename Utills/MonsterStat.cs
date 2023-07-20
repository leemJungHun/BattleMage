using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    public void InitStat(DefineHelper.ElementType type)
    {
        HP = 100;
        MaxHp = 100;
        switch (type)
        {
            case DefineHelper.ElementType.Fire:
                FireDefense = 50;
                WaterDefense = 0;
                LightningDefense = 65;
                break;
            case DefineHelper.ElementType.Water:
                FireDefense = 65;
                WaterDefense = 50;
                LightningDefense = 0;
                break;
            case DefineHelper.ElementType.Lightning:
                FireDefense = 0;
                WaterDefense = 65;
                LightningDefense = 50;
                break;
        }
    }
}
