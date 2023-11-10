using System.Collections;
using UnityEngine;
using gather;
using Gather.AI;
using UnityEngine.UI;

namespace Gather.UI
{
    public class NewGameScreen : MonoBehaviour
    {
        GameEventObject createGameEvent;
        Blackboard context;
        Dropdown botSelect;
        Dropdown colorSelect;

        public void Submit()
        {
            context.SetValue("botTeams", botSelect.value);
            createGameEvent.Raise();
        }

        public void SetContext(Blackboard bb)
        {
            context = bb;
        }
    }
}