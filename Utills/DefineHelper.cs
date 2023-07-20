using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineHelper
{
    #region[Manager Info]
    public enum _eSceneIndex
    {
        none,
        MainScene,
        IngameScene
    }
    public enum eIngameState
    {
        none = 0,
        PLAY,
        END,
        RESULT,

        state_Count
    }

    public enum eUIwindowtype
    {
        LoaddingWnd = 0,
        ResultWnd,
        OptionWnd,

        max_count
    }
    #endregion

    public enum ElementType
    {
        Fire = 0,
        Water,
        Lightning
    }

    public enum eBgmType
    {
        Main = 0,
        Ingame,

        max_count
    }
    public enum eFxType
    {
        Tap = 0,
        Counting,
        Hit,
        Dead,
        Defense
    }

}
