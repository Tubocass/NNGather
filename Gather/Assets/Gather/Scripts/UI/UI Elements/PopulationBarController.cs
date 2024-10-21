using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class PopulationBarController : MonoBehaviour
    {
        [SerializeField] float minLineWidth = 40f;
        int total = 0;
        TeamConfig[] teams;
        PopulationBarSegment[] segments;
        VisualElement fillBar;

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            fillBar = root.Q<VisualElement>(name: "PopulationBar");
            fillBar.Clear();
        }

        public virtual void SetupPopulationBar(TeamConfig[] teams)
        {
            this.teams = teams;
            segments = new PopulationBarSegment[teams.Length];
            for (int t = 0; t < segments.Length; t++)
            {
                segments[t] = new PopulationBarSegment();
                segments[t].fillColor = teams[t].TeamColor;
                segments[t].lineWidth = fillBar.contentRect.height > 0 ? fillBar.contentRect.height : minLineWidth;
                fillBar.Add(segments[t]);
            }
        }

        protected void FixedUpdate()
        {
            CalcTotalPopulation();
            float edge = 0;
            float width = fillBar.contentRect.width;
            for (int t = 0; t < teams.Length; t++)
            {
                Vector2 start = new Vector2(edge/2, 0);
                segments[t].transform.position = start;
                segments[t].fillAmount = CalcTeamPercent(t) * width; ;
                edge += segments[t].fillAmount;
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
            return Mathf.Clamp01(Mathf.InverseLerp(0, total, teams[team].UnitManager.GetTeamCount()));
        }
    }
}