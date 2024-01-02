using UnityEngine;
using TMPro;
using gather;
using UnityEditor;

namespace Gather.UI
{
    public class Custom_DropDown : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown colorPicker;
        [SerializeField] ColorOptions colorOptions;

        // Use this for initialization
        void Start()
        {
            Sprite square = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Gather/Sprites/ColoredSquare.png");

            colorPicker.ClearOptions();
            for (int i = 0; i < colorOptions.colors.Length; i++)
            {
                colorPicker.options.Add(
                    new TMP_Dropdown.OptionData(i.ToString(), square, colorOptions.colors[i])
                    );
            }
            colorPicker.value = 0;
            colorPicker.RefreshShownValue();
        }
    }
}