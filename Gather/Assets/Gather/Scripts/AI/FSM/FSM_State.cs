﻿using System.Collections.Generic;
using UnityEngine;

namespace Gather.AI
{
    public abstract class FSM_State 
    {
        public List<FSM_Transistion> transistions = new List<FSM_Transistion>();
        public virtual void EnterState() { }
        public virtual void Update() { }
        public virtual void ExitState() { }
        public virtual string GetStateName() { return null; }
    }
}