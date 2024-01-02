using System.Collections.Generic;
using UnityEngine;
using gather;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

namespace Gather.UI
{
    public class NewGameScreen : MonoBehaviour
    {
        [SerializeField] GameObject emptySlot;
        [SerializeField] GameObject teamSlotPrefab;
        [SerializeField] Transform listParent;

        //[SerializeField] ColorOptions colorOptions;
        List<GameObject> teamSlots = new List<GameObject>();
        //List<TeamSelect> selections = new List<TeamSelect>();

        private void Start()
        {
            GameObject go = Instantiate(teamSlotPrefab, listParent);
            //go.transform.SetAsFirstSibling();
            teamSlots.Add(go);
            //selections.Add(go.GetComponent<TeamSlot>().GetSelection());
            emptySlot.transform.SetAsLastSibling();
        }

        public void AddRow()
        {
            GameObject go = teamSlots.Find(f => !f.activeSelf);

            if (go)
            {
                go.SetActive(true);
            } else
            {
                go = Instantiate(teamSlotPrefab, listParent);
                teamSlots.Add(go);
                //selections.Add(go.GetComponent<TeamSlot>().GetSelection());
            }
            emptySlot.transform.SetAsLastSibling();

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