using UnityEngine;
using System.Collections;

public class LaserPlayerDetection : MonoBehaviour
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
		if (BeamOn ())
		{
			if (CollidedWithPlayer (other))
			{	
				lastPlayerSighting.position = other.transform.position;
			}
		}
	}

	bool BeamOn ()
	{
		return renderer.enabled;
	}

	bool CollidedWithPlayer (Collider other)
	{
		return other.gameObject == player;
	}
}