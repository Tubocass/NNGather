using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class PopulationBarController : MonoBehaviour
    {
        int total = 0;
        protected TeamConfig[] teams;
        PopulationBarElement[] elements;
        VisualElement fillBar;

        public virtual void SetTeams(TeamConfig[] teams)
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            fillBar = root.Q<VisualElement>(name: "PopulationBar");
            fillBar.Clear();
            this.teams = teams;
            elements = new PopulationBarElement[teams.Length];
            for (int t = 0; t < elements.Length; t++)
            {
                elements[t] = new PopulationBarElement();
                elements[t].fillColor = teams[t].TeamColor;
                fillBar.Add(elements[t]);
            }
        }

        protected void Update()
        {
            if (teams != null)
            {
                CalcTotalPopulation();
                float edge = 0;
                float width = fillBar.contentRect.width;
                for (int t = 0; t < teams.Length; t++)
                {

                    if (t == 0)
                    {
                        elements[t].fillAmount = CalcTeamPercent(t) * width;
                        edge += elements[t].fillAmount;
                        //elements[t].style.width = edge / 2;

                    } else
                    {
                        Vector2 start = new Vector2(edge/2, 0);
                        elements[t].transform.position = start;
                        elements[t].fillAmount = CalcTeamPercent(t) * width; ;
                        edge += elements[t].fillAmount;
                        //elements[t].style.width = edge / 2;
                    }
                }
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