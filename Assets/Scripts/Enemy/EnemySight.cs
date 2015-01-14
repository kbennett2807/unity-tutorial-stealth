using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour
{
	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public Vector3 personalLastPlayerSighting;
	private NavMeshAgent nav;
	private SphereCollider senseCollider;
	private Animator anim;
	private LastPlayerSighting globalLastPlayerSighting;
	private GameObject player;
	private Animator playerAnim;
	private PlayerHealth playerHealth;
	private HashIDs hash;
	private Vector3 previousGlobalPlayerSighting;
	
	void Awake ()
	{
		nav = GetComponent<NavMeshAgent> ();
		senseCollider = GetComponent<SphereCollider> ();
		anim = GetComponent<Animator> ();
		globalLastPlayerSighting = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<LastPlayerSighting> ();
		player = GameObject.FindGameObjectWithTag (Tags.player);
		playerAnim = player.GetComponent<Animator> ();
		playerHealth = player.GetComponent<PlayerHealth> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();

		personalLastPlayerSighting = globalLastPlayerSighting.resetPosition;
		previousGlobalPlayerSighting = globalLastPlayerSighting.resetPosition;
	}
	
	void Update ()
	{
		CheckGlobalPlayerSighting ();

		AnimateIfPlayerInSight ();
	}

	void AnimateIfPlayerInSight ()
	{
		if (playerHealth.health > 0f)
		{
			anim.SetBool (hash.playerInSightBool, playerInSight);
		}
		else
		{
			anim.SetBool (hash.playerInSightBool, false);
		}
	}

	void CheckGlobalPlayerSighting ()
	{
		if (NewGlobalPlayerSighting ())
		{
			personalLastPlayerSighting = globalLastPlayerSighting.position;
		}
		previousGlobalPlayerSighting = globalLastPlayerSighting.position;
	}

	bool NewGlobalPlayerSighting ()
	{
		return globalLastPlayerSighting.position != previousGlobalPlayerSighting;
	}
	
	void OnTriggerStay (Collider other)
	{
		if (other.gameObject == player)
		{
			CheckIfPlayerSeen (other);						
			CheckIfPlayerHeard ();			
		}
	}

	void CheckIfPlayerSeen (Collider other)
	{
		playerInSight = false;
		Vector3 directionOfPlayer = other.transform.position - transform.position;
		float angleOfPlayerFromForward = Vector3.Angle (directionOfPlayer, transform.forward);
		if (PlayerInFieldOfView (angleOfPlayerFromForward))
		{
			if (PlayerNotObstructed (directionOfPlayer))
			{
				playerInSight = true;
				globalLastPlayerSighting.position = player.transform.position;
			}
		}
	}

	bool PlayerInFieldOfView (float angleOfPlayerFromForward)
	{
		return angleOfPlayerFromForward < fieldOfViewAngle * 0.5f;
	}

	bool PlayerNotObstructed (Vector3 directionOfPlayer)
	{
		RaycastHit hit;
		return Physics.Raycast (transform.position + transform.up, directionOfPlayer.normalized, out hit, senseCollider.radius) && hit.collider.gameObject == player;
	}

	void CheckIfPlayerHeard ()
	{
		if (PlayerRunning () || PlayerAttractingAttention ())
		{
			if (PlayerInHearingRange ())
			{
				personalLastPlayerSighting = player.transform.position;
			}
		}
	}

	bool PlayerRunning ()
	{
		int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo (0).nameHash;
		return playerLayerZeroStateHash == hash.locomotionState;
	}

	bool PlayerAttractingAttention ()
	{
		int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo (1).nameHash;
		return playerLayerOneStateHash == hash.shoutState;
	}

	bool PlayerInHearingRange ()
	{
		return CalculatePathLength (player.transform.position) <= senseCollider.radius;
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject == player)
		{		
			playerInSight = false;
		}
	}
	
	float CalculatePathLength (Vector3 targetPosition)
	{
		NavMeshPath path = CalculatePathToPosition (targetPosition);
		Vector3[] allWayPointsOnPathFromEnemyToTarget = GetWayPointsOnPath (targetPosition, path);			
		return CalculatePathLength (allWayPointsOnPathFromEnemyToTarget);
	}

	NavMeshPath CalculatePathToPosition (Vector3 position)
	{
		NavMeshPath path = new NavMeshPath ();
		if (nav.enabled)
		{
			nav.CalculatePath (position, path);
		}
		return path;
	}

	Vector3[] GetWayPointsOnPath (Vector3 targetPosition, NavMeshPath path)
	{
		Vector3[] allWayPointsOnPathFromEnemyToTarget = new Vector3[path.corners.Length + 2];
		
		allWayPointsOnPathFromEnemyToTarget [0] = transform.position;
		allWayPointsOnPathFromEnemyToTarget [allWayPointsOnPathFromEnemyToTarget.Length - 1] = targetPosition;
		
		for (int i = 0; i < path.corners.Length; i++)
		{
			allWayPointsOnPathFromEnemyToTarget [i + 1] = path.corners [i];
		}

		return allWayPointsOnPathFromEnemyToTarget;
	}

	float CalculatePathLength (Vector3[] points)
	{
		float pathLength = 0;
		
		for (int i = 0; i < points.Length - 1; i++)
		{
			pathLength += Vector3.Distance (points [i], points [i + 1]);
		}

		return pathLength;
	}

}