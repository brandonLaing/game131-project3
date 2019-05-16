using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathControl))]
public class WalkableFloorEditor : Editor
{
  private static Tool lastTool = Tool.None;
  private bool isHelpOpen = false;

  public static void StoreTool()
  {
    lastTool = Tools.current;
    Tools.current = Tool.View;  // Only one we can eat left mouse drag with
  }

  public static void RestoreTool()
  {
    Tools.current = lastTool;
    lastTool = Tool.None;
  }

  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    // Enable renderers on waypoints
    foreach (Transform waypoint in (target as PathControl).transform)
    {
      if (waypoint.name.StartsWith("Waypoint_"))
      {
        waypoint.Find("Cylinder").gameObject.SetActive(true);
      }
    }
  }

  private void OnDestroy()
  {
    if (target == null) return; // ?
    foreach (Transform waypoint in (target as PathControl).transform)
    {
      if (waypoint.name.StartsWith("Waypoint_"))
      {
        waypoint.Find("Cylinder").gameObject.SetActive(false);
      }
    }
  }

  private void OnSceneGUI()
  {
    // Handle mouse clicks
    Event evt = Event.current;

    switch (evt.type)
    {
      case EventType.MouseDown:
        if (evt.button == 0)
        {
          if (HandleMouseDown(evt.mousePosition)) evt.Use();
        }
        break;
      case EventType.MouseUp:
        if (evt.button == 0)
        {
          RestoreTool();
        }
        break;
      case EventType.KeyDown:
        if (evt.keyCode == KeyCode.L)
        {
          ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        }
        break;
    }
    isHelpOpen = ShowHelpControls(isHelpOpen);
  }

  public static bool ShowHelpControls(bool helpOpen)
  {
    Handles.BeginGUI();
    if (helpOpen)
    {
      if (GUI.Button(new Rect(5, 5, 20, 20), "X")) helpOpen = false;
      List<string> helpLines = new List<string>();
      helpLines.Add("Click a walkable surface to add a new waypoint");
      helpLines.Add("Click-drag waypoints to move them");
      helpLines.Add("Press \"T\" to select Path from waypoint");
      helpLines.Add("Press \"L\" to lock inspector on path");
      GUI.Box(new Rect(5, 30, 400, 200), string.Join("\n", helpLines));
    }
    else
    {
      if (GUI.Button(new Rect(5, 5, 20, 20), "?")) helpOpen = true;
    }
    Handles.EndGUI();
    return helpOpen;
  }

  bool HandleMouseDown(Vector2 mousePosition)
  {
    // Did I get the path or a waypoint?
    // Create first.

    // Find what you clicked on:
    RaycastHit info;
    if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(mousePosition), out info))
    {
      if (info.collider.gameObject.GetComponent<WalkableFloor>() != null)
      {
        // Hit the floor; create a Waypoint
        GameObject waypointTemplate = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Waypoint.prefab", typeof(GameObject)) as GameObject;
        GameObject newWaypoint = PrefabUtility.InstantiatePrefab(waypointTemplate) as GameObject;
        newWaypoint.transform.position = info.point;
        newWaypoint.transform.parent = (this.target as PathControl).transform;
        (this.target as PathControl).ResetWaypointNames();


        // Cheat!
        StoreTool();

        GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);

        // Select it right away
        Selection.activeObject = newWaypoint;

        return true;
      }

      if (info.transform.parent != null && info.transform.parent.gameObject.tag == "Waypoint")
      {
        // Cheat!
        StoreTool();
        GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);

        // Select that waypoint
        Selection.activeObject = info.transform.parent.gameObject;
        return true;
      }
    }
    return false;
  }
}
