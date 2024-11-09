using UnityEngine;
using UnityEngine.UI;

namespace Gather.UI.Canvas
{
    public class PopulationBar : MonoBehaviour
    {
        protected TeamConfig[] teams;
        Image[] images;
        [SerializeField] GameObject fillBar;
        int total = 0;

        public virtual void SetTeams(TeamConfig[] teams)
        {
            this.teams = teams;
            images = new Image[teams.Length];
            for (int t = 0; t < images.Length; t++)
            {
                images[t] = Instantiate(fillBar, transform).GetComponent<Image>();
                images[t].color = teams[t].TeamColor;
            }
        }

        protected void Update()
        {
            if (teams != null)
            {
                DrawBar();
            }
        }

        protected virtual void DrawBar()
        {
            CalcTotalPopulation();
            float edge = 0;
            for (int t = 0; t < teams.Length; t++)
            {
                if (t == 0)
                {
                    images[t].fillAmount = CalcTeamPercent(t);
                    edge += images[t].rectTransform.rect.width * images[t].fillAmount;
                } else
                {
                    images[t].fillAmount = CalcTeamPercent(t);
                    images[t].transform.localPosition = new Vector3(edge, 0, 0);
                    edge += images[t].rectTransform.rect.width * images[t].fillAmount;
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
