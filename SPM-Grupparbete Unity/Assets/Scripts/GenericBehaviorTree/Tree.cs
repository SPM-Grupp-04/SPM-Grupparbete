using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private TreeNode root = null;

        protected void Start()
        {
            root = SetUpTree();
        }

        private void Update()
        {
            root?.Evaluate(); // kollar om det är null
        }

      
        protected abstract TreeNode SetUpTree();
    }
}