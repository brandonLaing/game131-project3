using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Waypoint : MonoBehaviour
{
  private void OnDrawGizmosSelected()
  {
    transform.parent.GetComponent<PathControl>().DrawPathGizmos();
  }

  private void OnDestroy()
  {
    transform.parent.GetComponent<PathControl>().ResetWaypointNames();
  }
}
