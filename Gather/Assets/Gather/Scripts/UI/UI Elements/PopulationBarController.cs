﻿using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class PopulationBarController : MonoBehaviour
    {
        [SerializeField] float minLineHeight = 40f;
        float barWidth;
        int total = 0;
        TeamConfig[] teams;
        LineSegment[] segments;
        VisualElement fillBar;

        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            fillBar = root.Q<VisualElement>(name: "PopulationBar");
            fillBar.Clear();
            fillBar.RegisterCallback<GeometryChangedEvent>(RetrieveSize);
        }

        void RetrieveSize(GeometryChangedEvent evt)
        {
            barWidth = fillBar.contentRect.width;

        }

        public virtual void SetupPopulationBar(TeamConfig[] teams)
        {
            this.teams = teams;
            segments = new LineSegment[teams.Length];
            
            for (int t = 0; t < segments.Length; t++)
            {
                segments[t] = new LineSegment();
                segments[t].fillColor = teams[t].TeamColor;
                segments[t].lineWidth = fillBar.contentRect.height > 0 ? fillBar.contentRect.height : minLineHeight;
                fillBar.Add(segments[t]);
            }
        }

        protected void FixedUpdate()
        {
            CalcTotalPopulation();
            float edge = 0;
            Vector2 start = Vector2.zero;
            for (int t = 0; t < teams.Length; t++)
            {
                start.x = edge / 2;
                segments[t].transform.position = start;
                segments[t].fillAmount = CalcTeamPercent(t) * barWidth; ;
                edge += segments[t].fillAmount;
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