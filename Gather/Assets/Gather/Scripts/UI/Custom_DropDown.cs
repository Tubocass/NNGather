using UnityEngine;
using TMPro;
using gather;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Gather.UI
{
    public class Custom_DropDown : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] TMP_Dropdown colorPicker;
        [SerializeField] ColorOptions colorOptions;
        [SerializeField] Sprite square;

        void Awake()
        {
            colorPicker.ClearOptions();

            for (int i = 0; i < colorOptions.colors.Length; i++)
            {
                colorPicker.options.Add(
                    new TMP_Dropdown.OptionData(i.ToString(), square, colorOptions.colors[i])
                    );
            }
            colorPicker.RefreshShownValue();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var list = GetComponentInChildren<Canvas>();
            if (list)
            {
                var toggles = list.GetComponentsInChildren<Toggle>();
                foreach (Toggle toggle in toggles)
                {
                    if (colorOptions.Contains(toggle.transform.GetSiblingIndex()-1))
                    {
                        toggle.interactable = false;
                    }
                }
            }
        }
    }
}