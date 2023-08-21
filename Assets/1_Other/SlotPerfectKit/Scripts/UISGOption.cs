using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BE {
	
	public class UISGOption : MonoBehaviour {
		
		private static UISGOption instance;
		
		public 	Toggle 		uiMusicToggle;
		public 	Toggle 		uiSoundToggle;
		public	Image 		Dialog;

		void Awake () {
			instance=this;
			gameObject.SetActive(false);
		}
		
		void Start () {
		}
		
		void Update () {
			if (Input.GetKeyDown(KeyCode.Escape)) { 
				Hide();
			}
		}
		
		void OnEnable(){
			uiMusicToggle.isOn = (BESetting.MusicVolume != 0) ? false : true;
			uiSoundToggle.isOn = (BESetting.SoundVolume != 0) ? false : true;
		}
		
		public void MusicToggled(bool value) {
			//Debug.Log ("MusicToggled "+value);
			BESetting.MusicVolume = value ? 0 : 100;
			BESetting.Save();
			
		}
		
		public void SoundToggled(bool value) {
			BESetting.SoundVolume = value ? 0 : 100;
			BESetting.Save();
		}
		
		public void OnButtonMenu() {
			Application.LoadLevel ("Lobby");
		}
		
		public void OnButtonContinue() {
			//Debug.Log("UISGOption::OnButtonContinue");
			//Hide();
		}
		
		public void Hide() {
			//Debug.Log("UISGOption::Hide");
			gameObject.SetActive(false);
			SceneSlotGame.uiState = 0;
		}

		void _Show () {
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.SetActive(true);
			SceneSlotGame.uiState = 1;
			StartCoroutine(BEUtil.instance.ImageScale(Dialog, Dialog.color, 1.0f, 1.1f, 1.0f, 0.1f, 0.0f));
		}
		
		public static void Show() { instance._Show(); }
		
	}
	
}