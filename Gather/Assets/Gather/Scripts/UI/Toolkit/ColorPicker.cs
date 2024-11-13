using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI.Toolkit
{
    [UxmlElement]
    public partial class ColorPicker : VisualElement, INotifyValueChanged<int>
    {
        public new class UxmlFactory : UxmlFactory<ColorPicker, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription m_Value = new UxmlIntAttributeDescription { name = "value" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                //get { yield break; }
                get { yield return new UxmlChildElementDescription(typeof(VisualElement)); }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((ColorPicker)ve).value = m_Value.GetValueFromBag(bag, cc);
            }
        }

        VisualElement[] options;
        VisualElement container => this.Q(name: "container");
        ColorOptions colorOptions;
        int index = -1;

        public int value { 
            get => index;
            set { 
                if(value == this.value)
                    return;

                var previous = this.value;
                SetValueWithoutNotify(value);
                using var evt = ChangeEvent<int>.GetPooled(previous, value);
                evt.target = this;
                SendEvent(evt);
            }
        }

        public ColorPicker() { }

        public void Init(ColorOptions colorOptions)
        {
            this.colorOptions = colorOptions;
            RegisterCallback<PointerDownEvent>(OnPointerDown);
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
                    container.style.display = DisplayStyle.None; 
                });
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
            UpdateChosenColor(index);
        }

        void UpdateChosenColor(int change)
        {
            this.style.backgroundColor = colorOptions.colors[change];
        }
    }
}