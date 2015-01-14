﻿using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
	public float maximumDamage = 120f;                  
	public float minimumDamage = 45f;                   
	public AudioClip shotClip;                          
	public float flashIntensity = 3f;                   
	public float fadeSpeed = 10f;                       

	private Animator anim;                              
	private HashIDs hash;                               
	private LineRenderer laserShotLine;                 
	private Light laserShotLight;                       
	private SphereCollider col;                         
	private Transform player;                           
	private PlayerHealth playerHealth;                  
	private bool shooting;                              
	private float scaledDamage;                         
	
	
	void Awake ()
	{
		anim = GetComponent<Animator> ();
		laserShotLine = GetComponentInChildren<LineRenderer> ();
		laserShotLight = laserShotLine.gameObject.light;
		col = GetComponent<SphereCollider> ();
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		playerHealth = player.gameObject.GetComponent<PlayerHealth> ();
		hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();
			
		laserShotLine.enabled = false;
		laserShotLight.intensity = 0f;

		scaledDamage = maximumDamage - minimumDamage;
	}
	
	void Update ()
	{
		float shot = anim.GetFloat (hash.shotFloat);

		if (shot > 0.5f && !shooting)
		{		
			Shoot ();
		}

		if (shot < 0.5f)
		{		
			shooting = false;
			laserShotLine.enabled = false;
		}
		

		laserShotLight.intensity = Mathf.Lerp (laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
	}
	
	void OnAnimatorIK (int layerIndex)
	{
		// Cache the current value of the AimWeight curve.
		float aimWeight = anim.GetFloat (hash.aimWeightFloat);
		
		// Set the IK position of the right hand to the player's centre.
		anim.SetIKPosition (AvatarIKGoal.RightHand, player.position + Vector3.up);
		
		// Set the weight of the IK compared to animation to that of the curve.
		anim.SetIKPositionWeight (AvatarIKGoal.RightHand, aimWeight);
	}
	
	void Shoot ()
	{
		// The enemy is shooting.
		shooting = true;
		
		// The fractional distance from the player, 1 is next to the player, 0 is the player is at the extent of the sphere collider.
		float fractionalDistance = (col.radius - Vector3.Distance (transform.position, player.position)) / col.radius;
		
		// The damage is the scaled damage, scaled by the fractional distance, plus the minimum damage.
		float damage = scaledDamage * fractionalDistance + minimumDamage;
		
		// The player takes damage.
		playerHealth.TakeDamage (damage);
		
		// Display the shot effects.
		ShotEffects ();
	}
	
	void ShotEffects ()
	{
		// Set the initial position of the line renderer to the position of the muzzle.
		laserShotLine.SetPosition (0, laserShotLine.transform.position);
		
		// Set the end position of the player's centre of mass.
		laserShotLine.SetPosition (1, player.position + Vector3.up * 1.5f);
		
		// Turn on the line renderer.
		laserShotLine.enabled = true;
		
		// Make the light flash.
		laserShotLight.intensity = flashIntensity;
		
		// Play the gun shot clip at the position of the muzzle flare.
		AudioSource.PlayClipAtPoint (shotClip, laserShotLight.transform.position);
	}
}