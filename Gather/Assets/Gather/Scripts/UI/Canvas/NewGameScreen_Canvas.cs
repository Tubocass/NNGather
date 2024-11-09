using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gather.UI
{
    public class NewGameScreen_Canvas : MonoBehaviour
    {
        [SerializeField] GameObject emptySlot;
        [SerializeField] GameObject teamSlotPrefab;
        [SerializeField] Transform listParent;
        List<GameObject> teamSlots = new List<GameObject>();
        [SerializeField] int maxTeams;

        private void Start()
        {
            GameObject go = Instantiate(teamSlotPrefab, listParent);
            go.GetComponent<TeamSlot>().SetPlayer(true);
            teamSlots.Add(go);
            emptySlot.transform.SetAsLastSibling();
        }

        public void AddRow()
        {
            GameObject go = teamSlots.Find(f => !f.activeSelf);

            if (go)
            {
                go.SetActive(true);
            } else if (teamSlots.Count < maxTeams)
            {
                go = Instantiate(teamSlotPrefab, listParent);
                teamSlots.Add(go);
             
            }
            go.GetComponent<TeamSlot>().SetPlayer(false);
            emptySlot.transform.SetAsLastSibling();

            if (teamSlots.FindAll(ts => ts.activeSelf).Count == maxTeams)
            {
                emptySlot.SetActive(false);
            }
        }

        public void RestoreEmptySlot()
        {
            emptySlot.SetActive(true);
        }

        public void Submit()
        {
            PlayerPrefs.SetInt("teamCount", teamSlots.Count);
            for (int i = 0; i < teamSlots.Count; i++) 
            {
                PlayerPrefs.SetString("team"+i, JsonUtility.ToJson(teamSlots[i].GetComponent<TeamSlot>().GetSelection()));

            }
            SceneManager.LoadScene("Gather");
        }
    }
}