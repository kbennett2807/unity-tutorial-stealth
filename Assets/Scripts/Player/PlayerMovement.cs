﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public AudioClip shoutingClip;
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;
	private Animator anim;
	private HashIDs hash;

	const float approximateMoveAnimationSpeed = 5.5f;
	
	void Awake ()
	{
		anim = GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();

		anim.SetLayerWeight (1, 1f);
	}
	
	void FixedUpdate ()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		bool sneak = Input.GetButton ("Sneak");
		
		MovementManagement (h, v, sneak);
	}
	
	void Update ()
	{
		bool shout = Input.GetButtonDown ("Attract");

		anim.SetBool (hash.shoutingBool, shout);
		
		AudioManagement (shout);
	}
	
	void MovementManagement (float horizontal, float vertical, bool sneaking)
	{
		anim.SetBool (hash.sneakingBool, sneaking);

		if (IsMovementInput (horizontal, vertical))
		{
			Rotate (horizontal, vertical);
			anim.SetFloat (hash.speedFloat, approximateMoveAnimationSpeed, speedDampTime, Time.deltaTime);
		}
		else
		{
			anim.SetFloat (hash.speedFloat, 0);
		}
	}

	static bool IsMovementInput (float horizontal, float vertical)
	{
		return horizontal != 0f || vertical != 0f;
	}
	
	void Rotate (float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3 (horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp (rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);

		rigidbody.MoveRotation (newRotation);
	}
	
	void AudioManagement (bool shout)
	{
		if (AnimatingLocomotion ())
		{
			if (!audio.isPlaying)
			{
				audio.Play ();
			}
		}
		else
		{
			audio.Stop ();
		}

		if (shout)
		{
			AudioSource.PlayClipAtPoint (shoutingClip, transform.position);
		}
	}

	bool AnimatingLocomotion ()
	{
		return anim.GetCurrentAnimatorStateInfo (0).nameHash == hash.locomotionState;
	}
}