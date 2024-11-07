using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] MaxCounter data;
        [SerializeField] Color fillColor;
        VisualElement container;
        VisualElement line;

        //private void Awake()
        //{
        //    var root = GetComponent<UIDocument>().rootVisualElement;
        //    VisualElement healthBar = root.Q<VisualElement>(name: "HealthBar");
        //    SetUIElement(healthBar);
        //}

        public void SetFillContainer(VisualElement element)
        {
            this.container = element;
            line = new VisualElement();
            container.Add(line);
            line.style.backgroundColor = fillColor;
            line.StretchToParentSize();
        }

        public void SetData(MaxCounter data) { 
            this.data = data;
            data.counterEvent.AddListener(UpdateLine);
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
