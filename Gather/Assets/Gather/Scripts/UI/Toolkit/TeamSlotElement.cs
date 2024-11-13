using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Gather.UI.Toolkit
{
    [System.Serializable]
    public struct TeamSelect
    {
        public int id;
        public bool isPlayer;
        public int colorIndex;
        public TeamSelect(int id, bool isPlayer, int colorOption)
        {
            this.id = id;
            this.isPlayer = isPlayer;
            this.colorIndex = colorOption;
        }
    }
    [UxmlElement]
    public partial class TeamSlotElement : VisualElement
    {
        public UnityEvent RowRemoved = new UnityEvent();
        TeamSelect selection = new TeamSelect(-1, false, -1);
        Toggle botSelect;
        Label playerText;
        ColorPicker colorSelect;
        ColorOptions colorOptions;
        NewGameScreen newGameScreen;

        public TeamSlotElement () { }

        public void Init (ColorOptions colors)
        {
            colorOptions = colors;
            botSelect = this.Q<Toggle>();
            playerText = botSelect.Q<Label>();
            colorSelect = this.Q<ColorPicker>();
            Button remove = this.Q<Button>();

            remove.clicked += RemoveRow;
            botSelect.RegisterValueChangedCallback(ChangePlayer);
            colorSelect.Init(colors);
            colorSelect.RegisterValueChangedCallback(SetColor);


            selection.id = parent.IndexOf(this);
            selection.isPlayer = botSelect.value;
            playerText.text = botSelect.value ? "Player" : "Bot";

            colorSelect.value = colorOptions.GetFreeColor();

            RegisterCallback<DetachFromPanelEvent>((evt) => OnDisable());
        }

        private void OnDisable()
        {
            colorOptions.DeselectColor(selection.colorIndex);
        }

        public void SetColor(ChangeEvent<int> change)
        {
            if (change.newValue != selection.colorIndex)
            {
                colorOptions.DeselectColor(change.previousValue);
                colorOptions.SelectColor(change.newValue);
                selection.colorIndex = change.newValue;
                //Debug.Log("Changed selected color");
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
            RowRemoved?.Invoke();
        }
    }
}