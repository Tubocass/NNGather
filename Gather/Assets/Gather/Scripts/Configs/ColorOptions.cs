using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gather
{
    [Serializable]
    public struct ColorOption
    {
        public Color color;
        public bool isSelected;
        public ColorOption(Color color, bool selected)
        {
            this.color = color;
            this.isSelected = selected;
        }
    }

    [CreateAssetMenu]
    public class ColorOptions : ScriptableObject
    {
        public ColorOption[] colorOptions;
        public Color[] colors;
        List<int> disabledOptions = new List<int>();

        private void OnEnable()
        {
            disabledOptions.Clear();
            for (int i = 0; i < colorOptions.Length; i++)
            {
                colorOptions[i].isSelected = false;
            }
        }

        public void DeselectColor(Color choice)
        {
            for (int i = 0; i < colorOptions.Length; i++)
            {
                if (choice.Equals(colorOptions[i].color))
                {
                    colorOptions[i].isSelected = false;
                }
            }
        }

        public void SelectColor(Color choice)
        {
            for (int i = 0; i < colorOptions.Length; i++)
            {
                if (choice.Equals(colorOptions[i].color))
                {
                    colorOptions[i].isSelected = true;
                }
            }
        }

        public Color GetFreeColorOption()
        {
            for (int i = 0; i < colorOptions.Length; i++)
            {
                if (!colorOptions[i].isSelected)
                {
                    return colorOptions[i].color;
                }
            }
            return Color.white;
        }

        public bool IsColorSelected(Color choice)
        {
            for (int i = 0; i < colorOptions.Length; i++)
            {
                if (choice.Equals(colorOptions[i].color))
                {
                    return colorOptions[i].isSelected;
                }
            }
            return false;
        }

        public void DeselectColor(int choice)
        {
            if (disabledOptions.Contains(choice))
            {
                disabledOptions.Remove(choice);
            }
        }

        public void SelectColor(int choice)
        {
            if (!disabledOptions.Contains(choice))
            {
                disabledOptions.Add(choice);
            }
        }

        public int GetFreeColor()
        {
            for (int i = 0; i < colors.Length; i++)
            {
                if(!Contains(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Contains(int index)
        {
            return disabledOptions.Contains(index);
        }
    }
}