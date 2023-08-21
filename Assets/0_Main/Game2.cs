using System;
using System.Collections;
using System.Collections.Generic;
using BE;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game2 : MonoBehaviour
{

    
    [Header("Main")]
    [SerializeField] private int bet;
    [SerializeField] private int delayForNewGame;
    [SerializeField] private CreditController creditController;
    [SerializeField] private Animation chestAnimation;
    [SerializeField] private Transform content;

    [Header("Prefabs")]
    [SerializeField] private GameObject winChest;
    [SerializeField] private GameObject loseChest;

    [Header("UI")]
    [SerializeField] private Text creditResultText;
    [SerializeField] private Text resultText;
    [SerializeField] private Text resultLabel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button menuButton;
    
    [Header("Audio")]
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip openChest;

    private void Start()
    {
        UIEnable(false);
    }
    
    private void OnEnable()
    {
        //UIEnable(false);
        Debug.Log(creditController.Credits);
        creditController.UpdateText((int) creditController.Credits);
    }

    public void OpenChest()
    {
        chestAnimation.Play();
        var win = Random.Range(0, 2) == 0;
        StartCoroutine(Result(win, chestAnimation.clip.length));
        Destroy(AudioManager.PlayAudio(openChest), openChest.length);
    }

    private IEnumerator Result(bool win, float delay = 0)
    {
        menuButton.interactable = false;
        openButton.interactable = false;
        yield return new WaitForSeconds(delay);
        UIEnable(true);
        var obj = win ? ShowResult(winChest, bet, "WIN", winClip) :
            ShowResult(loseChest, -bet, "LOSE", loseClip);
        
        yield return new WaitForSeconds(delayForNewGame);
        UIEnable(false);
        menuButton.interactable = true;;
        openButton.interactable = true;
        Destroy(obj);
    }

    private GameObject ShowResult(GameObject chest, int addedCredit, string result, AudioClip clip = null)
    {
        creditController.AddCredits(addedCredit);
        creditResultText.text = addedCredit.ToString();
        resultText.text = result;
        Destroy(AudioManager.PlayAudio(clip), clip.length);
        return Instantiate(chest, content);
    }

    private void UIEnable(bool enable)
    {
        creditResultText.gameObject.SetActive(enable);
        resultText.gameObject.SetActive(enable);
        resultLabel.gameObject.SetActive(enable);
        openButton.gameObject.SetActive(!enable);
        chestAnimation.gameObject.SetActive(!enable);
        chestAnimation.transform.Find("Image").GetComponent<Image>().color = new Color(1, 1, 1, !enable ? 0 : 1);
    }
}
