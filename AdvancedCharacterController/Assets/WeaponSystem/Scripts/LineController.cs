
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private Transform[] points;
    private bool set=false;
    
    public void SetLine(Transform[] points) {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = points.Length;
        this.points = points;
        set = true;
    }

    private void Update() {
        if (set) {
            for (int i = 0; i < points.Length; i++) {
                lr.SetPosition(i,points[i].position);
            }   
        }
    }
}
