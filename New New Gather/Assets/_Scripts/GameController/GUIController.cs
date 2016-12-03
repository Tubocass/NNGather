using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIController : MonoBehaviour 
{
	[SerializeField] RectTransform PauseScreen, ScoreScreen, NotificationPanel;
	[SerializeField] Text foodText, healthText, statText1, statText2, notificationText;
	MainMomController mainMoMControl;

	void OnEnable()
	{
		UnityEventManager.StartListeningInt("UpdateFood", SetFood);
		UnityEventManager.StartListeningInt("UpdateHealth", SetHealth);
		UnityEventManager.StartListeningInt("MoMDeath", Notify);
		StartCoroutine(UpdateInfo());
//		healthText.text =  "Health: " + 0;
//		scoreText.text =  "Food: " + 0;
	}
	void OnDisable()
	{
		UnityEventManager.StopListeningInt("UpdateFood", SetFood);
		UnityEventManager.StopListeningInt("UpdateHealth", SetHealth);
	}
	public void EnablePause(bool show)
	{
		PauseScreen.gameObject.SetActive(show);
	}
	void Notify(int team)
	{
		StartCoroutine(ShowNote(team+1));
	}
	IEnumerator ShowNote(int team)
	{
		NotificationPanel.gameObject.SetActive(true);
		notificationText.text = string.Format("Team {0} wiped out.", team);
		yield return new WaitForSeconds(4f);
		notificationText.text = "";
		NotificationPanel.gameObject.SetActive(false);

	}
	void SetFood(int amount)//called by event
	{
		foodText.text = "Food\n   "+ amount;
	}
	void SetHealth(int amount)//called by event
	{
		healthText.text = "Health\n   "+ amount;
	}

	IEnumerator UpdateInfo()
	{
		while(true)
		{
			float t1 = Unit_Base.TeamSize[0];
			float t2 = Unit_Base.TeamSize[1];
			float t3 = Unit_Base.TeamSize[2];
			float totalPop = t1+t2+t3;
			float t1Percent = Mathf.Floor(t1/totalPop*100);
			float t2Percent = Mathf.Floor(t2/totalPop*100);
			float t3Percent = Mathf.Floor(t3/totalPop*100);

			if(MainMomController.MainMoM!= null)
			{
				statText1.text = "Farmers: "+ MainMomController.MainMoM.farmers+ "\nFighters: "+ MainMomController.MainMoM.fighters+ "\nDaughters: "+ MainMomController.MainMoM.daughters;
				statText2.text =  "Team 1: "+ t1 +" - "+ t1Percent + "%" +  "\nTeam 2: "+ t2 +" - "+t2Percent +"%" +"\nTeam 3: "+ t3 +" - "+ t3Percent +"%";
			}
			yield return new WaitForSeconds(1f);
		}

	}
}
