using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
	public float smooth = 1.5f;
	private Transform player;
	private Vector3 cameraPosistionRelativeToPlayer;
	private float cameraDistanceFromPlayer;
	private Vector3 newCameraPosition;
		
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;

		cameraPosistionRelativeToPlayer = transform.position - player.position;
		cameraDistanceFromPlayer = cameraPosistionRelativeToPlayer.magnitude - 0.5f;
	}
	
	void FixedUpdate ()
	{

		Vector3 standardCameraPosition = player.position + cameraPosistionRelativeToPlayer;
		Vector3 abovePlayerCameraPosition = player.position + Vector3.up * cameraDistanceFromPlayer;

		Vector3[] cameraPositions = new Vector3[5];

		cameraPositions [0] = standardCameraPosition;

		cameraPositions [1] = Vector3.Lerp (standardCameraPosition, abovePlayerCameraPosition, 0.25f);
		cameraPositions [2] = Vector3.Lerp (standardCameraPosition, abovePlayerCameraPosition, 0.5f);
		cameraPositions [3] = Vector3.Lerp (standardCameraPosition, abovePlayerCameraPosition, 0.75f);

		cameraPositions [4] = abovePlayerCameraPosition;

		foreach (Vector3 cameraPosition in cameraPositions)
		{
			if (CameraCanSeePlayerAtPosition (cameraPosition))
			{
				transform.position = Vector3.Lerp (transform.position, cameraPosition, smooth * Time.deltaTime);
				break;
			}
		}

		SmoothLookAt ();
	}
	
	bool CameraCanSeePlayerAtPosition (Vector3 checkPos)
	{
		RaycastHit hit;

		if (Physics.Raycast (checkPos, player.position - checkPos, out hit, cameraDistanceFromPlayer))
		{
			if (hit.transform != player)
			{			
				return false;
			}
		}

		return true;
	}
	
	void SmoothLookAt ()
	{
		Vector3 playerPositionRelativeToCamera = player.position - transform.position;
		Quaternion lookAtPlayerRotation = Quaternion.LookRotation (playerPositionRelativeToCamera, Vector3.up);
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAtPlayerRotation, smooth * Time.deltaTime);
	}
}
