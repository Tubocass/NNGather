﻿using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI.Toolkit
{
    [UxmlElement]
    public partial class LineSegment : VisualElement
    {
        float m_fillAmount;
        [UxmlAttribute] public float fillAmount { get => m_fillAmount; set { m_fillAmount = value; MarkDirtyRepaint(); } }
        [UxmlAttribute] public float lineWidth = 80f;
        [UxmlAttribute] public Color fillColor;

        public LineSegment() 
        { 
            generateVisualContent += DrawBar;
        }

        protected void DrawBar(MeshGenerationContext mgc)
        {
            var painter = mgc.painter2D;
            painter.strokeColor = fillColor;
            painter.lineWidth = lineWidth;

            Vector2 point = Vector2.zero;
            point.x = fillAmount;

            painter.BeginPath();
            painter.MoveTo(transform.position);
            painter.LineTo((Vector2)transform.position + point);
            painter.Stroke();
        }

    }
}