using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gather.UI;

namespace gather
{
    [System.Serializable]
    public struct TeamSelect
    {
        public int id;
        public bool isPlayer;
        public int colorOption;
    }

    public class TeamSlot : MonoBehaviour
    {
        TeamSelect selection;
        public Toggle botSelect;
        public TMP_Dropdown colorSelect;
        public ColorOptions colorOptions;
        NewGameScreen newGameScreen;

        private void Awake()
        {
            newGameScreen = GetComponentInParent<NewGameScreen>();
            botSelect.onValueChanged.AddListener(SetPlayer);
            colorSelect.onValueChanged.AddListener(SetColor);

            selection.id = transform.GetSiblingIndex();
            selection.isPlayer = botSelect.isOn;
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
            selection.isPlayer = choice;
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