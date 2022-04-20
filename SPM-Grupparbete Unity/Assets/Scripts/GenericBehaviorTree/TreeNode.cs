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

    public class TreeNode
    {
        protected NodeState state;
        public TreeNode parent;
        protected List<TreeNode> children = new List<TreeNode>();

        private Dictionary<string, object> dataContext = new Dictionary<string, object>();

        public TreeNode()
        {
            parent = null;
        }

        public TreeNode(List<TreeNode> children)
        {
            foreach (TreeNode child in children)
            {
                add(child);
            }
        }

        private void add(TreeNode treeNode)
        {
            treeNode.parent = this;
            children.Add(treeNode);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value)
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

            TreeNode treeNode = parent;

            while (treeNode != null)
            {
                bool cleared = treeNode.ClearData(key);
                if (cleared)
                {
                    return true;
                }

                treeNode = treeNode.parent;
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

            TreeNode treeNode = parent;

            while (treeNode != null)
            {
                value = treeNode.GetData(key);
                if (value != null)
                {
                    return value;
                }

                treeNode = treeNode.parent;
            }


            return null;
        }
    }
}