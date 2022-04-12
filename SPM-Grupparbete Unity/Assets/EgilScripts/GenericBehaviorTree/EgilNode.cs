using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class EgilNode
    {
        protected NodeState state;
        public EgilNode parent;
        protected List<EgilNode> children;

        private Dictionary<string, object> dataContext = new Dictionary<string, object>();

        public EgilNode()
        {
            parent = null;
        }

        public EgilNode(List<EgilNode> children)
        {
            foreach (EgilNode child in children)
            {
                add(child);
            }
        }

        private void add(EgilNode egilNode)
        {
            egilNode.parent = this;
            children.Add(egilNode);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(String key, object value)
        {
            dataContext[key] = value;
        }

        public bool ClearData(string key)
        {
            object value = null;
            if (dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            EgilNode egilNode = parent;

            while (egilNode != null)
            {
                bool cleared = egilNode.ClearData(key);
                if (cleared)
                {
                    return true;
                }

                egilNode = egilNode.parent;
            }
            return false;
        }

        public object GetData(string key)
        {
            object value = null;
            if (dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            EgilNode egilNode = parent;

            while (egilNode != null)
            {
                value = egilNode.GetData(key);
                if (value != null)
                {
                    return value;
                }

                egilNode = egilNode.parent;
            }


            return null;
        }
    }
}