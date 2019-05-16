using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class KinematicSeek : MonoBehaviour
{
  [Tooltip("Points charecter in direction of where they are going")]
  [SerializeField]
  private bool lookWhereYouAreGoing = true;
  [Tooltip("Speed in meters per second")]
  [SerializeField]
  private float moveSpeed = 1.0f;
  [Tooltip("Targed location")]
  public Vector3 destination;
  [Tooltip("Stopping range for getting close to destination")]
  [SerializeField]
  private float radiusOfSatisfaction = 0.05f;

  /// <summary>
  /// Controller in charge of moving charecter
  /// </summary>
  private CharacterController characterController;
  /// <summary>
  /// If charecter is at its target or not
  /// </summary>
  public bool IsAtTarget { get; private set; }


  // Get the charecter controller
  void Awake()
  {
    characterController = GetComponent<CharacterController>();
  }

  // Sets starting destination to its own location
  private void Start()
  {
    destination = transform.position;
  }

  void Update()
  {
    // Get destination ignoreing the y
    Vector3 target2d = new Vector3(destination.x, transform.position.y, destination.z);
    // Check if its at its destination
    if (Vector3.Distance(target2d, transform.position) <= radiusOfSatisfaction)
    {
      IsAtTarget = true;
      return;
    }
    IsAtTarget = false;

    // If it isnt at its destination move it in the direction of the destination
    Vector3 moveDirection = (target2d - transform.position).normalized;
    Vector3 move = moveDirection * (moveSpeed * Time.deltaTime);
    characterController.Move(move);

    if (lookWhereYouAreGoing)
      transform.LookAt(transform.position + move);
  }
}
