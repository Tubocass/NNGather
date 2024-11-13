using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI.Toolkit
{
    [UxmlElement]
    public partial class ColorPicker : VisualElement, INotifyValueChanged<Color>
    {
        public new class UxmlFactory : UxmlFactory<ColorPicker, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlColorAttributeDescription m_Value = new UxmlColorAttributeDescription { name = "value" };

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

        ColorChoice[] options;
        VisualElement container => this.Q(name: "container");
        ColorOptions colorOptions;
        Color index = Color.white;

        public Color value { 
            get => index;
            set { 
                if(value == this.value)
                    return;

                var previous = this.value;
                SetValueWithoutNotify(value);
                using var evt = ChangeEvent<Color>.GetPooled(previous, value);
                evt.target = this;
                SendEvent(evt);
            }
        }

        public ColorPicker() { }

        public void Init(ColorOptions colorOptions)
        {
            this.colorOptions = colorOptions;
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            //container.RegisterCallback<FocusOutEvent>((evt) => {
            //    var container = evt.target as VisualElement;
            //    container.style.display = DisplayStyle.None;
            //    Debug.Log("Focus lost");
            //    });
            options = new ColorChoice[colorOptions.colorOptions.Length];

            for (int i = 0; i < options.Length; i++)
            {
                options[i] = new ColorChoice();
                options[i].AddToClassList("color-option");
                options[i].style.backgroundColor = colorOptions.colorOptions[i].color;
                container.Add(options[i]);
                
                options[i].RegisterCallback<PointerDownEvent>(SelectColor);
            }
        }

        void SelectColor(PointerDownEvent evt)
        {
            evt.StopPropagation();
            if((evt.currentTarget as VisualElement).parent == container )
            {
                ColorChoice element = evt.currentTarget as ColorChoice;
                Color selected = element.style.backgroundColor.value;
                if (!colorOptions.IsColorSelected(selected) || value == selected)
                {
                    value = selected;
                    container.style.display = DisplayStyle.None;
                }
            }
        }

        public void OnPointerDown(PointerDownEvent evt)
        {
            DisplayOptions();
        }

        public void DisplayOptions()
        {
            container.style.display = DisplayStyle.Flex;
            Color optionColor;
            for (int i = 0; i< options.Length; i++)
            {
                optionColor = options[i].style.backgroundColor.value;
                if (colorOptions.IsColorSelected(optionColor) && optionColor != value)
                {
                    options[i].Grayout();
                }else
                {
                    options[i].ClearMask();
                }
                if (optionColor == value)
                {
                    options[i].Highlight();
                }
            }
        }

        public void SetValueWithoutNotify(Color newValue)
        {
            index = newValue;
            UpdateChosenColor(newValue);
        }

        void UpdateChosenColor(Color change)
        {
            this.style.backgroundColor = change;
        }
    }
}