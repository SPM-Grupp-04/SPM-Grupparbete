using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
public class PlayerStatistics
{
  
   private static PlayerStatistics instance = null;

   private PlayerStatistics()
   {
   }

   public static PlayerStatistics Instance
   {
      get
      {
         if (instance == null)
         {
            instance = new PlayerStatistics();
         }
         return instance;
      }
   }
   
   //TODO: Att implemntera ett sätt att göra detta för två spelare.
   // Här kan Samtlig inofrmation om Antal kristaller ligga osv. All form av information som vill spara
   
   public float PlayerOnehp = 5;
   public float PlayerTwoHP = 5;
   public string Scene;
   public float PosX, PosY, PosZ;
   
}
