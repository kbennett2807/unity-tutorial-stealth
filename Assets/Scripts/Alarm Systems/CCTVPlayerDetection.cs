using UnityEngine;
using System.Collections;

public class CCTVPlayerDetection : MonoBehaviour
{
	private GameObject player;                          
	private LastPlayerSighting lastPlayerSighting;
		
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag (Tags.player);
		lastPlayerSighting = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<LastPlayerSighting> ();
	}
	
	void OnTriggerStay (Collider other)
	{
		// If the colliding gameobject is the player...
		if (CollidedWithPlayer (other))
		{
			Vector3 relativePlayerPos = player.transform.position - transform.position;
			RaycastHit hit;
			
			if (Physics.Raycast (transform.position, relativePlayerPos, out hit))
			{
				if (hit.collider.gameObject == player)
				{				
					lastPlayerSighting.position = player.transform.position;
				}
			}

		}
	}

	bool CollidedWithPlayer (Collider other)
	{
		return other.gameObject == player;
	}
}