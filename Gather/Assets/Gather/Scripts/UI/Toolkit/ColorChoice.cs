using UnityEngine.UIElements;

namespace Gather.UI.Toolkit 
{
    [UxmlElement]
    public partial class ColorChoice : VisualElement
    {
        readonly string highlightClass = "highlight";
        readonly string grayoutClass = "grayout";
        readonly VisualElement mask;

        public ColorChoice()
        {
            mask = new VisualElement();
            Add(mask);
        }

        public void Highlight()
        {
            mask.RemoveFromClassList(grayoutClass);
            mask.AddToClassList(highlightClass);
        }

        public void Grayout()
        {
            mask.RemoveFromClassList(highlightClass);
            mask.AddToClassList(grayoutClass);
        }

        public void ClearMask()
        {
            mask.ClearClassList();
        }
    }
}