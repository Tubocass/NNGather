using UnityEngine;
using UnityEngine.UI;

namespace gather
{
    public class PopulationBar : MonoBehaviour
    {
        [SerializeField] TeamConfig[] teams;
        Image[] image;
        [SerializeField] GameObject fillBar;
        int total = 0;


        private void Start()
        {
            image = new Image[teams.Length];
            for (int t = 0; t < image.Length; t++)
            {
                image[t] = Instantiate(fillBar, transform).GetComponent<Image>();
                image[t].color = teams[t].TeamColor;
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
                    image[t].fillAmount = FillTeam(t);
                    edge += image[t].rectTransform.rect.width * image[t].fillAmount;
                }
                else
                {
                    image[t].fillAmount = FillTeam(t);
                    image[t].transform.localPosition = new Vector3(edge, 0, 0);
                    edge += image[t].rectTransform.rect.width * image[t].fillAmount;
                }
            }
        }
    }
}
