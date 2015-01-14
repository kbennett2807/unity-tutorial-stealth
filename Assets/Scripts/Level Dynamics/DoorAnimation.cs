using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour
{
	public bool requireKey;
	public AudioClip doorSwishClip;
	public AudioClip accessDeniedClip;
	private Animator anim;
	private HashIDs hash;
	private GameObject player;
	private PlayerInventory playerInventory;
	private int numberOfPeopleInDoorRange;
	
	void Awake ()
	{
		anim = GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();
		player = GameObject.FindGameObjectWithTag (Tags.player);
		playerInventory = player.GetComponent<PlayerInventory> ();
	}
	
	void OnTriggerEnter (Collider other)
	{	
		if (IsPlayerCollider (other))
		{
			if (requireKey)
			{		
				if (playerInventory.hasKey)
				{
					numberOfPeopleInDoorRange++;
				}
				else
				{
					audio.clip = accessDeniedClip;
					audio.Play ();
				}
			}
			else
			{
				numberOfPeopleInDoorRange++;
			}							
		}
		else if (IsEnemyCollider (other))
		{						
			numberOfPeopleInDoorRange++;
		}
	}

	bool IsPlayerCollider (Collider other)
	{
		return other.gameObject == player;
	}
	
	void OnTriggerExit (Collider other)
	{
		if (IsPlayerCollider (other) || IsEnemyCollider (other))
		{		
			numberOfPeopleInDoorRange = Mathf.Max (0, numberOfPeopleInDoorRange - 1);
		}
	}

	static bool IsEnemyCollider (Collider other)
	{
		return (other.gameObject.tag == Tags.enemy && other is CapsuleCollider);
	}
	
	void Update ()
	{
		anim.SetBool (hash.openBool, numberOfPeopleInDoorRange > 0);

		if (anim.IsInTransition (0) && !audio.isPlaying)
		{		
			audio.clip = doorSwishClip;
			audio.Play ();
		}
	}
}