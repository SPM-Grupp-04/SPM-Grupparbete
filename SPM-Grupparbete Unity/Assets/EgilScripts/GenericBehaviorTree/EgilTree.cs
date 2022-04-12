using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class EgilTree : MonoBehaviour
    {
        private EgilNode root = null;

        protected void Start()
        {
            root = SetUpTree();
        }

        private void Update()
        {
            if (root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract EgilNode SetUpTree();
    }
}