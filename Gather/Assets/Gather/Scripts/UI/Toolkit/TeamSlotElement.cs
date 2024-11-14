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
        public Color teamColor;
        public TeamSelect(int id, bool isPlayer, int colorOption, Color color)
        {
            this.id = id;
            this.isPlayer = isPlayer;
            this.colorIndex = colorOption;
            this.teamColor = color;
        }
    }
    [UxmlElement]
    public partial class TeamSlotElement : VisualElement
    {
        public UnityEvent RowRemoved = new UnityEvent();
        TeamSelect selection = new TeamSelect(-1, false, -1, Color.white);
        Toggle botSelect => this.Q<Toggle>();
        Label playerText => botSelect.Q<Label>();
        ColorPicker colorSelect => this.Q<ColorPicker>();
        Button remove => this.Q<Button>();
        ColorOptions colorOptions;

        public TeamSlotElement () 
        {
            RegisterCallback<DetachFromPanelEvent>((evt) => OnDisable());
            RegisterCallback<AttachToPanelEvent>((evt) => colorOptions?.SelectColor(selection.teamColor));
        }

        public void Init (ColorOptions colors)
        {
            colorOptions = colors;
            colorSelect.Init(colors);
            colorSelect.RegisterValueChangedCallback(ChangeColor);
            colorSelect.value = colorOptions.GetFreeColorOption();
            remove.clicked += RemoveRow;
            botSelect.RegisterValueChangedCallback(ChangePlayer);

            selection.id = parent.IndexOf(this);
            selection.isPlayer = botSelect.value;
            playerText.text = botSelect.value ? "Player" : "Bot";
        }

        private void OnDisable()
        {
            colorOptions.DeselectColor(selection.teamColor);
        }

        public void ChangeColor(ChangeEvent<Color> change)
        {
            if (change.newValue != selection.teamColor)
            {
                colorOptions.DeselectColor(change.previousValue);
                colorOptions.SelectColor(change.newValue);
                selection.teamColor = change.newValue;
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
            RemoveFromHierarchy();
            RowRemoved?.Invoke();
        }
    }
}