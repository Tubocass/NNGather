using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI {
    public class UIDocumentController : MonoBehaviour
    {
        DisplayNumber foodLabel;

        public void SetupPlayerUI(Queen playerQueen, TeamConfig playerTeam)
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            Label foodCount = root.Q<Label>(name: "foodCount");
            foodLabel = new DisplayNumber(foodCount, playerQueen.GetFoodCounter());
        }
    }
}
