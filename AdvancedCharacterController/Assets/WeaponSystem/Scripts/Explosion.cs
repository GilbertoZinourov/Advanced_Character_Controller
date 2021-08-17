using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
   public GameObject smallSphere;
   public GameObject bigSphere;

   public void SetExplosion(float smallRadius, float bigRadius) {
      smallSphere.transform.localScale = new Vector3(smallRadius, smallRadius, smallRadius);
      bigSphere.transform.localScale = new Vector3(bigRadius, 0.01f, bigRadius);
   }
}
