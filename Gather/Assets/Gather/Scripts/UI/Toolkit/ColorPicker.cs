using UnityEngine.UIElements;

namespace Gather.UI.Toolkit
{
    [UxmlElement]
    public partial class ColorPicker : VisualElement, INotifyValueChanged<int>
    {
        VisualElement[] options;
        VisualElement container => this.Q(name: "container");
        ColorOptions colorOptions;
        int index;

        public int value { 
            get => index;
            set { 
                index = value;
                UpdateChosenColor(index);
            }
        }

        public ColorPicker() { }

        public void Init(ColorOptions colorOptions)
        {
            this.colorOptions = colorOptions;
            this.RegisterCallback<PointerDownEvent>(OnPointerDown);
            options = new VisualElement[colorOptions.colors.Length];

            for (int i = 0; i < options.Length; i++)
            {
                options[i] = new VisualElement();
                options[i].AddToClassList("color-option");
                options[i].style.backgroundColor = colorOptions.colors[i];
                container.Add(options[i]);
                
                int optionIndex = i;
                options[i].RegisterCallback<PointerDownEvent>((evt) => {
                    evt.StopPropagation();
                    value = optionIndex;
                    container.style.display = DisplayStyle.None; });
            }
        }

        public void OnPointerDown(PointerDownEvent evt)
        {
            DisplayOptions();
        }

        public void DisplayOptions()
        {
            container.style.display = DisplayStyle.Flex;
        }

        public void SetValueWithoutNotify(int newValue)
        {
            index = newValue;
        }

        void UpdateChosenColor(int change)
        {
            colorOptions.DeselectColor(change);
            colorOptions.SelectColor(change);
            this.style.backgroundColor = colorOptions.colors[change];
        }
    }
}