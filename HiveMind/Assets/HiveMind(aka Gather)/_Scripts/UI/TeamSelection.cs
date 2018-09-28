using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelection : MonoBehaviour
{
    public GameSetupManager manager;
    public Button colorButton;
    [SerializeField] Image colorBox;
    [SerializeField] Text nameText;
    public Color myColor;
    string myName;
    bool isHuman;
    int team, colorIndex;
   
    public void Setup(PlayerSelection player)
    {
        myColor = player.teamColor;
        myName = player.playerHandle;
        team = player.teamNumber;
        isHuman = player.isHuman;

        colorBox.color = player.teamColor;
        nameText.text = player.playerHandle;
    }
    public PlayerSelection AddPlayer()
    {
        PlayerSelection player = new PlayerSelection(isHuman, myName, team, myColor);
        return player;
    }
    public void ToggleActive(bool active)
    {
        manager.ConfirmPlayer(team, active);
    }
   public void ChangeColor()
    {
        colorBox.color = manager.SelectColor(team);
    }
    /*
     * OnToggle: Fade in/out row, add to number of players
     * Color Select: OnClick change to next AVAILABLE color in GameSetupManager, 
     * remember choice for player in Game
     * Name: Displays player/AI name
     * */
}
