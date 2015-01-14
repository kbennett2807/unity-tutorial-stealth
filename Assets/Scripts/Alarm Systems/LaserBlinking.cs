using UnityEngine;
using System.Collections;

public class LaserBlinking : MonoBehaviour
{
	public float secondsLaserIsOn;
	public float secondsLaserIsOff;
	private float timer;
	
	void Update ()
	{
		timer += Time.deltaTime;

		if (BeamOn () && timer >= secondsLaserIsOn)
		{
			SwitchBeam ();
		}

		if (!BeamOn () && timer >= secondsLaserIsOff)
		{		
			SwitchBeam ();
		}
	}

	bool BeamOn ()
	{
		return renderer.enabled;
	}
	
	void SwitchBeam ()
	{
		timer = 0f;

		renderer.enabled = !renderer.enabled;
		light.enabled = !light.enabled;
	}
}