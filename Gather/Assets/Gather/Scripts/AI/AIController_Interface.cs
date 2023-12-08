using System.Collections;
using UnityEngine;

namespace Gather.AI
{
    public interface AIController_Interface
    {
        public void Enable(int team);
        public void AssessSituation();
        public void Disable();

    }
}