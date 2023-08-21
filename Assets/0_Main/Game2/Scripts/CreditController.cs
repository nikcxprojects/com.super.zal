using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditController : MonoBehaviour
{

    private float _credits;
    public float Credits => PlayerPrefs.GetFloat("Credit");

    [SerializeField] private Text creditsText;

    private void Start()
    {
        _credits = PlayerPrefs.GetFloat("Credit");
        UpdateText();
    }

    public void UpdateText()
    {
        creditsText.text = _credits.ToString();
        PlayerPrefs.SetFloat("Credit", _credits);
    }
    
    public void UpdateText(int credits)
    {
        _credits = credits;
        creditsText.text = _credits.ToString();
        PlayerPrefs.SetFloat("Credit", _credits);
    }

    public void AddCredits(int credits)
    {
        _credits += credits;
        UpdateText();
    }
    
}
