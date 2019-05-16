using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KinematicSeek))]
public class WalkPath : MonoBehaviour
{
  public PathControl pathToWalk;
  private int waypointIndex = 0;
  private KinematicSeek kinematicSeek;
  private RotatingQue<Vector3> _waypoints;

  private void Awake()
  {
    kinematicSeek = GetComponent<KinematicSeek>();
  }

  void Start()
  {
    List<Vector3> waypointLocations = pathToWalk.Waypoints;
    for (int i = 1; i < waypointLocations.Count; i++)
    {
      // Get the closest point
      if (Vector3.Distance(transform.position, waypointLocations[i]) < Vector3.Distance(transform.position, waypointLocations[waypointIndex]))
        waypointIndex = i;
    }

    _waypoints = new RotatingQue<Vector3>(pathToWalk.Waypoints);
  }

  // Update is called once per frame
  void Update()
  {
    if (kinematicSeek.IsAtTarget)
      kinematicSeek.destination = _waypoints.Next;
  }
}

public class RotatingQue<T>
{
  private List<T> _que = new List<T>();
  private int currentIndex = 0;

  public RotatingQue(List<T> que, int startingIndex = 0)
  {
    _que = que;
    currentIndex = startingIndex;
  }

  public T Next
  {
    get
    {
      currentIndex = currentIndex + 1 < Count ? currentIndex + 1 : 0; 
      return _que[currentIndex];
    }
  }

  public int Count
  {
    get { return _que.Count; }
  }
}