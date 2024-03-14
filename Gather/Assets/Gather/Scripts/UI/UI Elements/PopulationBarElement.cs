using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class PopulationBarElement : PopulationBar
    {
        Image[] images;
        VisualElement fillBar;
        [SerializeField] Texture2D imageTexture;

        public override void SetTeams(TeamConfig[] teams)
        {
            this.teams = teams;

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            fillBar = root.Q<VisualElement>(name: "PopulationBar");
            images = new Image[teams.Length];

            for (int i = 0; i < teams.Length; i++)
            {
                images[i] = new Image(); 
                images[i].image = imageTexture;
                images[i].AddToClassList("populationSegment");
                fillBar.Add(images[i]);
            }
        }

        protected override void DrawBar()
        {
            CalcTotalPopulation();
            float edge = 0;
            for (int t = 0; t < teams.Length; t++)
            {
                if (t == 0)
                {
                    images[t].transform.scale =  new Vector3(CalcTeamPercent(t), 1, 1);
                    edge += images[t].image.width * images[t].transform.scale.x;
                    Debug.Log(edge);
                } else
                {
                    images[t].transform.scale = new Vector3(CalcTeamPercent(t), 1, 1);
                    //images[t].transform.position = new Vector3(edge, 0, 0);
                    edge += images[t].image.width * images[t].transform.scale.x;
                }
            }
        }
    }
}