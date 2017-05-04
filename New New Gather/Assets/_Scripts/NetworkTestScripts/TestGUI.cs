using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGUI : MonoBehaviour 
{
	[SerializeField] RectTransform PauseScreen, ScoreScreen, NotificationPanel, StatPanel;
	[SerializeField] Text foodText, healthText, statText1, statText2, notificationText;
	public Interact playerControl;

	void OnEnable()
	{
		StatPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)(30*2)+15);
//		UnityEventManager.StartListeningInt("UpdateFood", SetFood);
//		UnityEventManager.StartListeningInt("UpdateHealth", SetHealth);
//		UnityEventManager.StartListeningInt("MoMDeath", Notify);
		StartCoroutine(UpdateInfo());
	}
	void OnDisable()
	{
//		UnityEventManager.StopListeningInt("UpdateFood", SetFood);
//		UnityEventManager.StopListeningInt("UpdateHealth", SetHealth);
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
			if(playerControl!= null)
			{
				statText1.text = "Farmers: "+ playerControl.Units;
				statText2.text = "";
				for(int i = 0; i< TestGameController.instance.numPlayers;i++)
				{
					statText2.text += string.Format("\nTeam {0}: {1} - {2:F1}%", i+1, TestGameController.instance.Count[i], TestGameController.instance.TeamSizePercent(i));
				}
				//statText2.text =  "Team 1: "+ t1 +" - "+ t1Percent + "%" +  "\nTeam 2: "+ t2 +" - "+t2Percent +"%" +"\nTeam 3: "+ t3 +" - "+ t3Percent +"%";
			}
			yield return new WaitForSeconds(.5f);
		}
	}
}
