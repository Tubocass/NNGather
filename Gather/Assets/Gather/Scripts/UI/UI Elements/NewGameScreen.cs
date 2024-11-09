using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class NewGameScreen : MonoBehaviour
    {
        [SerializeField] ColorOptions colorOptions;
        [SerializeField] VisualTreeAsset teamSlotTemplate;
        List<TeamSlotElement> teamSlots = new List<TeamSlotElement>();
        [SerializeField] int maxTeams;
        VisualElement teamPanel;
        VisualElement emptySlot;


        private void Start()
        {
            teamPanel = GetComponent<UIDocument>().rootVisualElement
                .Q<VisualElement>(name: "teamPanel");
            emptySlot = teamPanel.Q(name: "emptySlot");
            Button add = emptySlot.Q<Button>();
            add.clicked += AddRow;
            add.clicked += () => Debug.Log("Clicked");
            CreateTeamSlot().SetPlayer(true);
            //emptySlot.transform.SetAsLastSibling();
            //teamPanel.BringToFront();
        }

        public void AddRow()
        {
            TeamSlotElement ts = teamSlots.Find(f => f.style.display == DisplayStyle.None);

            if (ts != null)
            {
                ts.style.display = DisplayStyle.Flex;
            } else if (teamSlots.Count < maxTeams)
            {
                ts = CreateTeamSlot();

            }
            ts.SetPlayer(false);
            //emptySlot.transform.SetAsLastSibling();

            if (teamSlots.FindAll(ts => ts.style.display == DisplayStyle.Flex).Count == maxTeams)
            {
                emptySlot.style.display = DisplayStyle.None;
            }
        }

        TeamSlotElement CreateTeamSlot()
        {
            var tempContainer = teamSlotTemplate.Instantiate();
            TeamSlotElement ts = tempContainer.Q<TeamSlotElement>();
            teamSlots.Add(ts);
            teamPanel.Add(ts);
            ts.Init(colorOptions);
            return ts;
        }

        public void RestoreEmptySlot()
        {
            emptySlot.style.display = DisplayStyle.Flex;
        }

        public void Submit()
        {
            PlayerPrefs.SetInt("teamCount", teamSlots.Count);
            for (int i = 0; i < teamSlots.Count; i++) 
            {
                PlayerPrefs.SetString("team"+i, JsonUtility.ToJson(teamSlots[i].GetSelection()));

            }
            SceneManager.LoadScene("Gather");
        }
    }
}