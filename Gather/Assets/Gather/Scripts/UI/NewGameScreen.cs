using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.UI
{
    public class NewGameScreen : MonoBehaviour
    {
        //GameEventObject createGameEvent;
        //Blackboard context;
        [SerializeField] GameObject emptySlotPrefab;
        [SerializeField] GameObject TeamSlotPrefab;
        [SerializeField] ColorOption colorOptions;
        List<GameObject> teamSlots = new List<GameObject>();
        List<TeamSelect> Teams = new List<TeamSelect>();

        [SerializeField] Transform listParent;

        private void Start()
        {
            GameObject go = Instantiate(TeamSlotPrefab, listParent);
            teamSlots.Add(go);
            Teams.Add(go.GetComponent<TeamSlot>().GetSelection());
        }

        public void AddRow()
        {
            GameObject go = teamSlots.Find(f => !f.activeSelf);

            if (go)
            {
                go.SetActive(true);
            } else
            {
                go = Instantiate(TeamSlotPrefab, listParent);
                teamSlots.Add(go);
                Teams.Add(go.GetComponent<TeamSlot>().GetSelection());
            }
            
        }

        public void Submit()
        {

            //createGameEvent.Raise();
        }
    }
}