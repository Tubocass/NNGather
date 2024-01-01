using UnityEngine;
using UnityEngine.UI;

namespace gather
{
    public class PopulationBar : MonoBehaviour
    {
        [SerializeField] TeamConfig[] teams;
        Image[] images;
        [SerializeField] GameObject fillBar;
        int total = 0;


        private void Start()
        {
            
        }

        public void SetTeams(TeamConfig[] teams)
        {
            this.teams = teams;
            images = new Image[teams.Length];
            for (int t = 0; t < images.Length; t++)
            {
                images[t] = Instantiate(fillBar, transform).GetComponent<Image>();
                images[t].color = teams[t].TeamColor;
            }
        }

        void CalcTotal()
        {
            total = 0;
            for (int t = 0; t < teams.Length; t++)
            {
                total += teams[t].GetTeamCount();
            }
        }
        float FillTeam(int team)
        {
            return Mathf.Clamp01(Mathf.InverseLerp(0, total, teams[team].GetTeamCount()));
        }

        private void Update()
        {
            CalcTotal();
            float edge = 0;
            for (int t = 0; t < teams.Length; t++)
            {
                if (t == 0)
                {
                    images[t].fillAmount = FillTeam(t);
                    edge += images[t].rectTransform.rect.width * images[t].fillAmount;
                }
                else
                {
                    images[t].fillAmount = FillTeam(t);
                    images[t].transform.localPosition = new Vector3(edge, 0, 0);
                    edge += images[t].rectTransform.rect.width * images[t].fillAmount;
                }
            }
        }
    }
}
