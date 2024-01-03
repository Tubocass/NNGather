using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    [CreateAssetMenu]
    public class ColorOptions : ScriptableObject
    {
        public Color[] colors;
        List<int> disabledOptions = new List<int>();

        private void OnEnable()
        {
            //disabledOptions.Clear();
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
                    Debug.Log(i);
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