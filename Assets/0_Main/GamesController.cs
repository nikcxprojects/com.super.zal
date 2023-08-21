using System.Collections;
using System.Collections.Generic;
using BE;
using UnityEngine;

public class GamesController : MonoBehaviour
{
    [SerializeField] private GameObject _miniGame1;
    [SerializeField] private GameObject _mainGame;
    [SerializeField] private GameObject _menu;

    [Header("Audio")] 
    [SerializeField] private AudioClip mainGame; 
    [SerializeField] private AudioClip miniGame;

    private GameObject _musicObjMain;
    private GameObject _musicObjMini; 

    [SerializeField] private CreditController[] _creditControllers;

    private void Start()
    {
        ShowMenu(true);
        Application.targetFrameRate = 60;
    }
    
    public void ShowMiniGame(bool value)
    {
        _miniGame1.SetActive(value);
        UpdateCredit();
        if (!value) return;
        _menu.SetActive(false);
        _mainGame.SetActive(false);
        _musicObjMini = AudioManager.PlayAudio(miniGame, Variables.SettingsFieldType.Music);
        if (_musicObjMain) Destroy(_musicObjMain);
    }

    public void ShowMainGame(bool value)
    {
        _mainGame.SetActive(value);
        UpdateCredit();
        if (!value) return;
        _menu.SetActive(false);
        _miniGame1.SetActive(false);
        _musicObjMain = AudioManager.PlayAudio(mainGame, Variables.SettingsFieldType.Music);
        if (_musicObjMini) Destroy(_musicObjMini);
        BESetting.Load();
    }

    public void ShowMenu(bool value)
    {
        _menu.SetActive(value);
        UpdateCredit();
        if (!value) return;
        _mainGame.SetActive(false);
        _miniGame1.SetActive(false);
        if (_musicObjMini) Destroy(_musicObjMini);
        if (_musicObjMain) Destroy(_musicObjMain);
    }

    public void ReshowCurrentPanel()
    {
        ShowMenu(_menu.activeSelf);
        ShowMainGame(_mainGame.activeSelf);
        ShowMiniGame(_miniGame1.activeSelf);
    }

    private void UpdateCredit()
    {
        foreach (var credit in _creditControllers)
        {
            credit.UpdateText((int) credit.Credits);
        }
    }
}
