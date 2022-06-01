using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetColor : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private void Start()
    {
        skinnedMeshRenderer = GameObject.Find("Players/" + this.name + "/Character2_New/player_model_low_new").GetComponent<SkinnedMeshRenderer>();
        if (this.name == "Player1")
        {
            Color color = skinnedMeshRenderer.material.color;
            color.r = PlayerStatistics.Instance.playerOneColor[0];
            color.g = PlayerStatistics.Instance.playerOneColor[1];
            color.b = PlayerStatistics.Instance.playerOneColor[2];
            skinnedMeshRenderer.material.color = color;
        }
        else
        {
            Color color = skinnedMeshRenderer.material.color;
            color.r = PlayerStatistics.Instance.playerTwoColor[0];
            color.g = PlayerStatistics.Instance.playerTwoColor[1];
            color.b = PlayerStatistics.Instance.playerTwoColor[2];
            skinnedMeshRenderer.material.color = color;
        }
    }
}
