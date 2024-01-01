using UnityEngine;
using UnityEngine.UI;

namespace gather
{
    public class TeamSlot : MonoBehaviour
    {
        TeamSelect selection;
        Dropdown botSelect;
        Dropdown colorSelect;

        void SetColor(int choice)
        {
            selection.colorOption.isSelected = true;
        }

        public TeamSelect GetSelection()
        {
            return selection;
        }
    }
}