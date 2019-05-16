using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathControl : MonoBehaviour
{
  private List<Vector3> waypointLocations = new List<Vector3>();
  public List<Vector3> Waypoints { get { return waypointLocations; } }

  // Grab all lower waypoints
  void Start()
  {
    foreach (Transform waypoint in transform)
    {
      if (waypoint.name.StartsWith("Waypoint_"))
      {
        waypointLocations.Add(waypoint.transform.position);
        waypoint.Find("Cylinder").gameObject.SetActive(false);
      }
    }
  }

  public void ResetWaypointNames()
  {
    int ix = 0;
    foreach (Transform waypoint in transform)
    {
      if (waypoint.name.StartsWith("Waypoint"))
      {
        waypoint.name = "Waypoint_" + ix.ToString("0000");
        ix++;
      }
    }
  }

  private void OnDrawGizmosSelected()
  {
    DrawPathGizmos();
  }

  public void DrawPathGizmos()
  {
    Color original = Gizmos.color;
    Gizmos.color = Color.green;
    List<Transform> waypoints = new List<Transform>();
    foreach (Transform waypoint in transform)
    {
      if (waypoint.name.StartsWith("Waypoint"))
      {
        waypoints.Add(waypoint);
      }
    }

    // Now draw paths
    for (int i = 0; i < waypoints.Count; i++)
    {
      Transform startWaypoint = waypoints[i];
      Transform endWaypoint = waypoints[(i + 1) < waypoints.Count ? i + 1 : 0];

      // Detect if there's anything in the way
      RaycastHit hitForward = new RaycastHit(), hitBack = new RaycastHit();

      if (Physics.Raycast(startWaypoint.position + Vector3.up, endWaypoint.position - startWaypoint.position, out hitForward)
        || Physics.Raycast(endWaypoint.position + Vector3Int.up, startWaypoint.position - endWaypoint.position, out hitBack))
      {
        // Green if we hit where we expect
        if ((hitForward.collider != null && hitForward.collider.transform.parent != null && hitForward.collider.transform != endWaypoint)
          || (hitBack.collider != null && hitBack.collider.transform.parent != null && hitBack.collider.transform != startWaypoint))
          Gizmos.color = Color.green;
        else
          Gizmos.color = Color.red;
      }
      else
        Gizmos.color = Color.green;

      Gizmos.DrawLine(startWaypoint.position, endWaypoint.position);

      // Draw the direction as arrows halfway along
      Vector3 center = (startWaypoint.position + endWaypoint.position) / 2;
      Vector3 startToEnd = (endWaypoint.position - startWaypoint.position).normalized;
      Vector3 lineNormal = new Vector3(startToEnd.z, startToEnd.y, -startToEnd.x);    // Cheap but effective hack for 2D normal
      Gizmos.DrawLine(center, center - startToEnd * 0.3f + lineNormal * 0.3f);    // Back and to one direction
      Gizmos.DrawLine(center, center - startToEnd * 0.3f - lineNormal * 0.3f);    // Back and to the other direction
    }
    Gizmos.color = original;
  }

}
