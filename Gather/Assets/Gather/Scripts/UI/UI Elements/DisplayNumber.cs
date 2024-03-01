using UnityEngine.UIElements;

namespace Gather.UI
{
    public class DisplayNumber
    {
        Label label;
        Counter counter;

        public DisplayNumber(Label label, Counter counter)
        {
            this.label = label;
            this.counter = counter;
            counter.counterEvent.AddListener(UpdateText);
            UpdateText();
        }

        public void UpdateText()
        {
            label.text = counter?.GetAmount().ToString();
        }
    }
}