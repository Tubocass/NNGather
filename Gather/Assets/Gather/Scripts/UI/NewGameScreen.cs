using System.Collections.Generic;
using UnityEngine;
using gather;
using Gather.AI;
using UnityEngine.UI;

namespace Gather.UI
{
    public class NewGameScreen : MonoBehaviour
    {
        //GameEventObject createGameEvent;
        //Blackboard context;
        [SerializeField] GameObject emptySlotPrefab;
        [SerializeField] GameObject TeamSlotPrefab;
     
        [SerializeField] ColorOption colorOptions;

        //int playerCount = 1;
        //int botCount;
        //List<TeamSelect> Teams = new List<TeamSelect>();

        public void AddRow()
        {

        }

        public void Submit()
        {
            //createGameEvent.Raise();
        }
    }
}