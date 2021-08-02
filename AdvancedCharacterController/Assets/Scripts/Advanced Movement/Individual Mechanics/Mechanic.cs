using System;
using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
   public class Mechanic : MonoBehaviour{
      protected PlayerMovement _pm;
      
      private void Awake(){
         _pm = GetComponent<PlayerMovement>();
      }

      private void Start(){
         CheckIfEnabled();
         VariablesSetUp();
      }

      protected virtual void OnEnable(){
         _pm.OnVariableChange += VariablesSetUp;
      }

      protected virtual void OnDisable(){
         _pm.OnVariableChange -= VariablesSetUp;
      }

      protected virtual void VariablesSetUp(){ }

      protected virtual void CheckIfEnabled(){ }
   }
}
