using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
	public float health = 100f;
	public float resetAfterDeathTime = 5f;
	public AudioClip deathClip;
	private Animator anim;
	private PlayerMovement playerMovement;
	private HashIDs hash;
	private SceneFadeInOut sceneFadeInOut;
	private LastPlayerSighting lastPlayerSighting;
	private float timer;
	private bool playerDead;

	public void TakeDamage (float amount)
	{
		health -= amount;
	}
	
	void Awake ()
	{

		anim = GetComponent<Animator> ();
		playerMovement = GetComponent<PlayerMovement> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();
		sceneFadeInOut = GameObject.FindGameObjectWithTag (Tags.fader).GetComponent<SceneFadeInOut> ();
		lastPlayerSighting = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<LastPlayerSighting> ();
	}
	
	void Update ()
	{
		if (health <= 0f)
		{
			if (!playerDead)
			{
				PlayerDying ();
			}
			else
			{
				PlayerDead ();
				LevelReset ();
			}
		}
	}
	
	void PlayerDying ()
	{
		playerDead = true;
		
		// Set the animator's dead parameter to true also.
		anim.SetBool (hash.deadBool, playerDead);
		
		// Play the dying sound effect at the player's location.
		AudioSource.PlayClipAtPoint (deathClip, transform.position);
	}
	
	void PlayerDead ()
	{
		if (InDyingAnimtionState ())
		{
			anim.SetBool (hash.deadBool, false);
		}

		anim.SetFloat (hash.speedFloat, 0f);
		playerMovement.enabled = false;

		lastPlayerSighting.SetPlayerLocationUnknown();

		audio.Stop ();
	}

	bool InDyingAnimtionState ()
	{
		return anim.GetCurrentAnimatorStateInfo (0).nameHash == hash.dyingState;
	}
	
	void LevelReset ()
	{
		timer += Time.deltaTime;

		if (timer >= resetAfterDeathTime)
		{
			sceneFadeInOut.EndScene ();
		}
	}
}