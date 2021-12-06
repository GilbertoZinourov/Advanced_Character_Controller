// MIT License
//
// Copyright (c) 2021 Gilberto Zinourov
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.



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
