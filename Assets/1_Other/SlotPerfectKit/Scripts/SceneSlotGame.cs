using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;


namespace BE 
{
	public class SceneSlotGame : MonoBehaviour 
	{

		public	static SceneSlotGame instance;

		public static int uiState = 0;
		public static BENumber	Win;

		public SlotGame	Slot;

		[SerializeField] private CreditController _creditController;

		[Header("Text")]
		[SerializeField] private Text textLine;
		[SerializeField] private Text textBet;	
		[SerializeField] private Text textTotalBet;
		[SerializeField] private Text textGold;
		
		[Space]
		[Space]
		[Header("Buttons")]
		
		[Space]
		[SerializeField] private Button	btnSpin;
		[SerializeField] private Toggle	toggleAutoSpin;		// start spin
		[Space]
		
		[Header("Bet")]
		[SerializeField] private Button	btnBetMinus;
		[SerializeField] private Button btnBetPlus;	
		
		[Space]
		
		[Header("Line")]

		[SerializeField] private Button	btnLinesMinus;
		[SerializeField] private Button	btnLinesPlus;
		
		[Header("Line")]
		[SerializeField] private AudioClip reelStopClip;
		[SerializeField] private AudioClip spinEndClip;
		[SerializeField] private AudioClip buttonClip;
		[SerializeField] private AudioClip errorClip;
		

		
		private void Awake () 
		{
			instance=this;
		}

		private void Start () 
		{
			BESetting.Gold.AddUIText(textGold);
			Win = new BENumber(BENumber.IncType.VALUE, 0, 10000000000, 2000);			

			Slot.Gold = _creditController.Credits;
			Win.ChangeTo(0);
			Invoke("UpdateUI", 0.5f);
		}

		private void OnEnable()
		{
			UpdateUI();
			Slot.Gold = _creditController.Credits;
		}

		void Update () 
		{
			if (uiState==0 && Input.GetKeyDown(KeyCode.Escape)) 
			{ 
				UISGMessage.Show("Quit", "Do you want to quit this program ?", MsgType.OkCancel, MessageQuitResult);
			}
			Win.Update();
			
			if((toggleAutoSpin.isOn || Slot.gameResult.InFreeSpin()) && Slot.Spinable()) 
			{
				OnButtonSpin();
			}
		}
		

		public void MessageQuitResult(int value) 
		{
			if(value == 0) Application.Quit ();
		}

		public void OnButtonLinePlus() 
		{
			Slot.LineSet(Slot.Line + 1);
			UpdateUI();
		}
		
		public void OnButtonLineMinus() 
		{
			Slot.LineSet(Slot.Line - 1);
			UpdateUI();
		}

		public void OnButtonBetPlus() 
		{
			Slot.BetSet(Slot.Bet+1);
			UpdateUI();
		}
		
		public void OnButtonBetMinus() 
		{
			Slot.BetSet(Slot.Bet-1);
			UpdateUI();
		}

		public void OnButtonSpin() 
		{
			AudioManager.PlayAudio(buttonClip);
			var code = Slot.Spin();
			switch (code)
			{
				case SlotReturnCode.Success:
					ButtonEnable(false);
					UpdateUI();
					BESetting.Gold.ChangeTo(Slot.Gold);
					break;
				case SlotReturnCode.InSpin:
					Destroy(AudioManager.PlayAudio(errorClip), errorClip.length);
					UISGMessage.Show("Error", "Slot is in spin now.", MsgType.Ok, null);
					break;
				case SlotReturnCode.NoGold:
					Destroy(AudioManager.PlayAudio(errorClip), errorClip.length);
					UISGMessage.Show("Error", "Not enough gold.", MsgType.Ok, null);
					break;
			}
		}

		public void AutoSpinToggled(bool value) 
		{
		}

		public void UpdateUI() 
		{
			Destroy(AudioManager.PlayAudio(buttonClip), buttonClip.length);
			textLine.text = Slot.Line.ToString (); 
			textBet.text = Slot.RealBet.ToString();
			textTotalBet.text = Slot.TotalBet.ToString();
			_creditController.UpdateText((int) Slot.Gold);
			if(Slot.gameResult.GameWin != null) Win.ChangeTo(Slot.gameResult.GameWin);
		}

		public void ButtonEnable(bool bEnable) 
		{
			btnLinesMinus.interactable = bEnable;
			btnLinesPlus.interactable = bEnable;
			btnBetMinus.interactable = bEnable;
			btnBetPlus.interactable = bEnable;
			btnSpin.interactable = bEnable; 
		}
		
		public void OnDoubleGameEnd(float delta) 
		{
			Slot.Gold += delta;
			BESetting.Gold.ChangeTo(Slot.Gold);
			BESetting.Save();
			Slot.gameResult.GameWin += delta;
			Win.ChangeTo(Slot.gameResult.GameWin);
		}

		public void OnReelStop(int value) 
		{
			Destroy(AudioManager.PlayAudio(reelStopClip), reelStopClip.length);
			//Debug.Log ("OnReelStop:"+value.ToString());
		}

		public void OnSpinEnd() 
		{
			Destroy(AudioManager.PlayAudio(spinEndClip), spinEndClip.length);
			UpdateUI();
			BESetting.Gold.ChangeTo(Slot.Gold);
		}
		
		public void OnSplashHide(int value) 
		{
			StartCoroutine(SlotSplashHide(value, 0.5f));
		}

		public void OnSplashEnd() 
		{
			ButtonEnable(true);

		}

		public IEnumerator SlotSplashHide(int value, float fDelay) 
		{
			if(fDelay > 0.01f) yield return new WaitForSeconds(fDelay);
			
			Slot.SplashCount[value] = 0;
			Slot.SplashActive++;
			Slot.InSplashShow = false;
		}

	}

}
