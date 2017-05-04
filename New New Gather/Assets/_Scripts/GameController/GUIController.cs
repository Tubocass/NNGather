using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIController : MonoBehaviour 
{
	[SerializeField] RectTransform PauseScreen, ScoreScreen, NotificationPanel, StatPanel;
	[SerializeField] Text foodText, healthText, statText1, statText2, notificationText;
	public MoMController mainMoMControl;

	void OnEnable()
	{
		UnityEventManager.StartListeningInt("UpdateFood", SetFood);
		UnityEventManager.StartListeningInt("UpdateHealth", SetHealth);
		UnityEventManager.StartListeningInt("MoMDeath", Notify);
		UnityEventManager.StartListening("MainMomChange",MoMChanged);
		StartCoroutine(UpdateInfo());
//		healthText.text =  "Health: " + 0;
//		scoreText.text =  "Food: " + 0;
	}
	void OnDisable()
	{
		UnityEventManager.StopListeningInt("UpdateFood", SetFood);
		UnityEventManager.StopListeningInt("UpdateHealth", SetHealth);
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			EnablePause();
		}
	}
	public void EnablePause()
	{
		PauseScreen.gameObject.SetActive(!PauseScreen.gameObject.activeSelf);
	}
	public void Escape()
	{
		Application.Quit();
	}
	public void SetTeams(int t)
	{
		StatPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)(30*t)+15);
	}
	void MoMChanged()
	{
		SetFood(mainMoMControl.FoodAmount);
		SetHealth((int)mainMoMControl.Health);
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
			if(mainMoMControl!= null && GameController.instance!=null)
			{
				SetTeams(GameController.instance.numPlayers);
				statText1.text = "Farmers: "+ mainMoMControl.farmers+ "\nFighters: "+ mainMoMControl.fighters+ "\nDaughters: "+ mainMoMControl.daughters;
				statText2.text = "";
				for(int i = 0; i< GameController.instance.numPlayers;i++)
				{
					statText2.text += string.Format("\nTeam {0}: {1} - {2:F1}%", i+1, GameController.instance.TeamSize[i], GameController.instance.TeamSizePercent(i));
				}
				//statText2.text =  "Team 1: "+ t1 +" - "+ t1Percent + "%" +  "\nTeam 2: "+ t2 +" - "+t2Percent +"%" +"\nTeam 3: "+ t3 +" - "+ t3Percent +"%";
			}
			yield return new WaitForSeconds(.5f);
		}

	}
}
