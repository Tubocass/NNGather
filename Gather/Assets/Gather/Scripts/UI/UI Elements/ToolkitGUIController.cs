using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI {
    public class ToolkitGUIController : GUIController
    {
        DisplayNumber foodLabel;

        public override void SetupPlayerUI(Queen playerQueen)
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            Label foodCount = root.Q<Label>(name: "foodCount");
            foodLabel = new DisplayNumber(foodCount, playerQueen.GetFoodCounter());
        }

        public override void SetupPopulationBar(TeamConfig[] teams)
        {
            throw new System.NotImplementedException();
        }
    }
}
