using UnityEngine;
using System.Collections;

public class LiftDoorsTracking : MonoBehaviour
{
	public float doorSpeed = 7f;
	private Transform leftOuterDoor;
	private Transform rightOuterDoor;
	private Transform leftInnerDoor;
	private Transform rightInnerDoor;
	private float leftInnerDoorClosedPositionX;
	private float rightInnerDoorClosedPositionX;
	
	void Awake ()
	{
		leftOuterDoor = GameObject.Find ("door_exit_outer_left_001").transform;
		rightOuterDoor = GameObject.Find ("door_exit_outer_right_001").transform;
		leftInnerDoor = GameObject.Find ("door_exit_inner_left_001").transform;
		rightInnerDoor = GameObject.Find ("door_exit_inner_right_001").transform;
			
		leftInnerDoorClosedPositionX = leftInnerDoor.position.x;
		rightInnerDoorClosedPositionX = rightInnerDoor.position.x;
	}
	
	void MoveDoors (float newLeftInnerDoorXTarget, float newRightInnerDoorXTarget)
	{
		float newXPosition = Mathf.Lerp (leftInnerDoor.position.x, newLeftInnerDoorXTarget, doorSpeed * Time.deltaTime);			
		leftInnerDoor.position = new Vector3 (newXPosition, leftInnerDoor.position.y, leftInnerDoor.position.z);

		newXPosition = Mathf.Lerp (rightInnerDoor.position.x, newRightInnerDoorXTarget, doorSpeed * Time.deltaTime);
		rightInnerDoor.position = new Vector3 (newXPosition, rightInnerDoor.position.y, rightInnerDoor.position.z);
	}
	
	public void DoorFollowing ()
	{
		MoveDoors (leftOuterDoor.position.x, rightOuterDoor.position.x);
	}
	
	public void CloseDoors ()
	{
		MoveDoors (leftInnerDoorClosedPositionX, rightInnerDoorClosedPositionX);
	}
}