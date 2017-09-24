using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIController : MonoBehaviour 
{
	[SerializeField] RectTransform PauseScreen, ScoreScreen, NotificationPanel, StatPanel;
	[SerializeField] Text foodText, healthText, statText1, statText2, notificationText;
	[SerializeField] Image foodDisplay, healthDisplay;
	public MoMController mainMoMControl;

	// FoodDisplay foodDisplay;
	// HealthDisplay healthDisplay;

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
		UnityEventManager.StopListening("MainMomChange",MoMChanged);
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
		if(mainMoMControl!=null)
		{
			SetFood(mainMoMControl.FoodAmount);
			SetHealth((int)mainMoMControl.Health);
			SetTeams(GameController.instance.TeamSize.Count);
			StartCoroutine(UpdateInfo());
		}
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
		foodDisplay.transform.localScale = new Vector3(1, (float)amount/mainMoMControl.maxFood, 1);
	}
	void SetHealth(int amount)//called by event
	{
		healthText.text = "Health\n   "+ amount;
		healthDisplay.transform.localScale = new Vector3(1, (float)amount/mainMoMControl.startHealth, 1);
	}

	IEnumerator UpdateInfo()
	{
		while(mainMoMControl!=null)
		{
			if(mainMoMControl!= null && GameController.instance!=null && GameController.instance.TeamSize.Count>0)
			{
				statText1.text = "Farmers: "+ mainMoMControl.farmers+ "\nFighters: "+ mainMoMControl.fighters+ "\nDaughters: "+ mainMoMControl.daughters;
				statText2.text = "";
				for(int i = 0; i< GameController.instance.numPlayers;i++)
				{
					statText2.text += string.Format("\nTeam {0}: {1} - {2:F1}%", i+1, GameController.instance.TeamSize[i], GameController.instance.TeamSizePercent(i));
				}
			}
			yield return new WaitForSeconds(.5f);
		}

	}
}
