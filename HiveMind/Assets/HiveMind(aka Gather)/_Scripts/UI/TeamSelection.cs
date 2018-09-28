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
        colorBox.color = player.teamColor;
        myColor = player.teamColor;
        nameText.text = player.name;
        team = player.teamNumber;
        isHuman = player.isHuman;
    }
    public PlayerSelection AddPlayer()
    {
        PlayerSelection player = new PlayerSelection(isHuman, name, team, myColor);
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
