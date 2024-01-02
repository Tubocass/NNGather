using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        private void Awake()
        {
            colorSelect.onValueChanged.AddListener(SetColor);
            botSelect.onValueChanged.AddListener(SetPlayer);
            selection.isPlayer = botSelect.isOn;
            selection.colorOption = colorSelect.value;
        }

        public void SetColor(int choice)
        {
            selection.colorOption = choice;
        }

        public void SetPlayer(bool choice) 
        {
            selection.isPlayer = choice;
        }

        public TeamSelect GetSelection()
        {
            return selection;
        }
    }
}