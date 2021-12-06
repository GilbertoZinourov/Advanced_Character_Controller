//    Advanced Character Controller
//    Copyright (C) 2021  Gilberto Zinourov
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.



using UnityEngine;

namespace Advanced_Movement.Individual_Mechanics{
   public class Mechanic : MonoBehaviour{
      protected AdvancedMovement _pm;
      
      private void Awake(){
         _pm = GetComponent<AdvancedMovement>();
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
