using UnityEngine;

namespace Gather.UI
{
    public abstract class GUIController: MonoBehaviour
    {
        public virtual void SetupPlayerUI(Queen playerQueen) { }
        public virtual void SetupPopulationBar(TeamConfig[] teams) { }

    }
}