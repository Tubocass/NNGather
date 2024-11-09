using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gather.UI;

namespace Gather
{
    //[System.Serializable]
    //public struct TeamSelect
    //{
    //    public int id;
    //    public bool isPlayer;
    //    public int colorOption;
    //    public TeamSelect(int id, bool isPlayer, int colorOption)
    //    {
    //        this.id = id;
    //        this.isPlayer = isPlayer;
    //        this.colorOption = colorOption;
    //    }
    //}

    public class TeamSlot : MonoBehaviour
    {
        TeamSelect selection;
        public Toggle botSelect;
        public TMP_Dropdown colorSelect;
        public ColorOptions colorOptions;
        NewGameScreen_Canvas newGameScreen;
        TMP_Text playerText;


        private void Awake()
        {
            playerText = botSelect.GetComponentInChildren<TMP_Text>();
            newGameScreen = GetComponentInParent<NewGameScreen_Canvas>();
            botSelect.onValueChanged.AddListener(SetPlayer);
            colorSelect.onValueChanged.AddListener(SetColor);

            selection.id = transform.GetSiblingIndex();
            selection.isPlayer = botSelect.isOn;
            playerText.text = botSelect.isOn ? "Player" : "Bot";
        }

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            selection.colorOption = colorOptions.GetFreeColor();
            colorSelect.value = selection.colorOption;
            colorOptions.SelectColor(colorSelect.value);
        }

        private void OnDisable()
        {
            colorOptions.DeselectColor(selection.colorOption);
        }

        public void SetColor(int choice)
        {
            if (choice != selection.colorOption)
            {
                colorOptions.DeselectColor(selection.colorOption);
                selection.colorOption = choice;
                colorOptions.SelectColor(choice);
            }
        }

        public void SetPlayer(bool choice) 
        {
            botSelect.isOn = choice;
            selection.isPlayer = choice;
            playerText.text = choice ? "Player" : "Bot";
        }

        public TeamSelect GetSelection()
        {
            return selection;
        }

        public void RemoveRow()
        {
            gameObject.SetActive(false);
            newGameScreen.RestoreEmptySlot();
        }
    }
}