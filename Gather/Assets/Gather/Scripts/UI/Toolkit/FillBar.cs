using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI.Toolkit
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] MaxCounter data;
        [SerializeField] Color fillColor;
        VisualElement container;
        VisualElement line;

        public void Initialize(VisualElement element, MaxCounter data)
        {
            this.container = element;
            this.data = data;

            line = new VisualElement();
            container.Add(line);
            line.style.backgroundColor = fillColor;
            line.StretchToParentSize();

            data.counterEvent.AddListener(UpdateLine);
            UpdateLine();
        }

        private void UpdateLine()
        {
            if (container != null)
            {
                line.style.width = Mathf.InverseLerp(0, data.GetMax(), data.GetAmount()) * container.contentRect.width;
            }
        }

    }
}
