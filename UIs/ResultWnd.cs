using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWnd : MonoBehaviour
{

    [SerializeField] Text _txtTotalScore;
    [SerializeField] Image[] _starImages;
    [SerializeField] Sprite[] _starSprites;

    //정보변수
    int _maxCombo = 0;
    int _heart = 0;
    float _drawCombo = 0;
    float _countingTime = 2;
    bool _gameClear = false;

    void LateUpdate()
    {
        if (_gameClear)
        {
            _maxCombo = InGameManager._instance.MaxCombo;
            _heart = InGameManager._instance.StageHeart;
            if (_maxCombo <= _drawCombo)
            {
                _txtTotalScore.text = _maxCombo + "Combo";
                _starImages[0].sprite = _heart >= 1 ? _starSprites[0] : _starSprites[1];
                _starImages[1].sprite = _heart >= 2 ? _starSprites[0] : _starSprites[1];
                _starImages[2].sprite = _heart >= 3 ? _starSprites[0] : _starSprites[1];
            }
            else
            {
                _drawCombo += _maxCombo * (Time.deltaTime / _countingTime);
                _txtTotalScore.text = (int)_drawCombo + "Combo";
                _starImages[0].sprite = _heart >= 1 ? _starSprites[0] : _starSprites[1];
                _starImages[1].sprite = _heart >= 2 ? _starSprites[0] : _starSprites[1];
                _starImages[2].sprite = _heart >= 3 ? _starSprites[0] : _starSprites[1];
            }
        }
       
    }

    public void ClickOkButton()
    {
        SoundManager.instance.PlaySfxSoundOneShot(DefineHelper.eFxType.Tap);
        SceneControlManager._instance.StartMainScene();
    }

    public void ClickExitButton()
    {
        SoundManager.instance.PlaySfxSoundOneShot(DefineHelper.eFxType.Tap);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenResultWindow(int totalScore, bool gameClear)
    {
        _gameClear = gameClear;
        if (gameClear)
        {
            _txtTotalScore.text = "0 Combo";
            _maxCombo = totalScore;
            SoundManager.instance.PlaySfxSoundOneShot(DefineHelper.eFxType.Counting);

        }
        else
        {
            _txtTotalScore.text = "Failed";
            _txtTotalScore.color = Color.red;
            for(int i = 0; i< _starImages.Length; i++)
            {
                _starImages[i].gameObject.SetActive(false);
            }
        }
    }
}
