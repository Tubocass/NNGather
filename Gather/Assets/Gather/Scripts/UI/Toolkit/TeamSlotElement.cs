using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Gather.UI.Toolkit
{
    [System.Serializable]
    public struct TeamSelect
    {
        public int id;
        public bool isPlayer;
        public int colorOption;
        public TeamSelect(int id, bool isPlayer, int colorOption)
        {
            this.id = id;
            this.isPlayer = isPlayer;
            this.colorOption = colorOption;
        }
    }
    [UxmlElement]
    public partial class TeamSlotElement : VisualElement
    {
        public UnityEvent RowRemoved = new UnityEvent();
        TeamSelect selection;
        Toggle botSelect;
        Label playerText;
        DropdownField colorSelect;
        ColorOptions colorOptions;
        NewGameScreen newGameScreen;

        public TeamSlotElement () { }

        public void Init (ColorOptions colors)
        {
            colorOptions = colors;
            botSelect = this.Q<Toggle>();
            playerText = botSelect.Q<Label>();
            colorSelect = this.Q<DropdownField>();
            Button remove = this.Q<Button>();

            remove.clicked += RemoveRow;
            botSelect.RegisterValueChangedCallback(ChangePlayer);
            colorSelect.RegisterCallback<ChangeEvent<int>>(SetColor);

            selection.id = parent.IndexOf(this);
            selection.isPlayer = botSelect.value;
            playerText.text = botSelect.value ? "Player" : "Bot";

            selection.colorOption = colorOptions.GetFreeColor();
            colorSelect.index = selection.colorOption;
            colorOptions.SelectColor(colorSelect.index);
        }

        //private void OnDisable()
        //{
        //    colorOptions.DeselectColor(selection.colorOption);
        //}

        public void SetupColorOptions(ColorOptions colorOptions)
        {
        }

        public void SetColor(ChangeEvent<int> change)
        {
            int choice = change.newValue;
            if (choice != selection.colorOption)
            {
                colorOptions.DeselectColor(selection.colorOption);
                selection.colorOption = choice;
                colorOptions.SelectColor(choice);
            }
        }

        public void ChangePlayer(ChangeEvent<bool> choice)
        {
            SetPlayer(choice.newValue);
        }

        public void SetPlayer(bool choice) 
        {
            botSelect.value = choice;
            selection.isPlayer = choice;
            playerText.text = choice ? "Player" : "Bot";
        }

        public TeamSelect GetSelection()
        {
            return selection;
        }

        public void RemoveRow()
        {
            this.style.display = DisplayStyle.None;
            if(RowRemoved != null)
            {
                RowRemoved.Invoke();
            }
        }
    }
}