using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class PopulationBarController : MonoBehaviour
    {
        int total = 0;
        float edge = 0;
        Vector2 start = Vector2.zero;
        TeamConfig[] teams;
        VisualElement[] segments;
        VisualElement container;

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            container = root.Q<VisualElement>(name: "populationBar");
            container.Clear();
        }

        public virtual void SetupPopulationBar(TeamConfig[] teams)
        {
            this.teams = teams;
            segments = new VisualElement[teams.Length];
            
            for (int t = 0; t < segments.Length; t++)
            {
                segments[t] = new VisualElement();
                segments[t].style.backgroundColor = teams[t].TeamColor;
                container.Add(segments[t]);
                segments[t].StretchToParentSize();
            }
        }

        protected void LateUpdate()
        {
            CalcTotalPopulation();
            edge = 0;
            
            for (int t = 0; t < teams.Length; t++)
            {
                start.x = edge;
                segments[t].transform.position = start;
                segments[t].style.width = CalcTeamPercent(t) * container.contentRect.width;
                edge += segments[t].style.width.value.value;
            }
        }

        protected void CalcTotalPopulation()
        {
            total = 0;
            for (int t = 0; t < teams.Length; t++)
            {
                total += teams[t].UnitManager.GetTeamCount();
            }
        }

        protected float CalcTeamPercent(int team)
        {
            return Mathf.InverseLerp(0, total, teams[team].UnitManager.GetTeamCount());
        }
    }
}