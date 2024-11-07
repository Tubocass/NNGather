using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] MaxCounter data;
        [SerializeField] float lineWidth;
        [SerializeField] Color fillColor;
        VisualElement container;
        LineSegment line;

        //private void Awake()
        //{
        //    var root = GetComponent<UIDocument>().rootVisualElement;
        //    VisualElement healthBar = root.Q<VisualElement>(name: "HealthBar");
        //    SetUIElement(healthBar);
        //}

        public void SetUIElement(VisualElement element)
        {
            this.container = element;
            line = new LineSegment();
            line.fillColor = fillColor;
            line.lineWidth = lineWidth;
            container.Add(line);
        }

        public void SetData(MaxCounter data) { this.data = data; }

        private void FixedUpdate()
        {
            if (container != null)
            {
                line.fillAmount = Mathf.InverseLerp(0, data.GetMax(), data.GetAmount()) * container.contentRect.width;
            }
        }

    }
}
