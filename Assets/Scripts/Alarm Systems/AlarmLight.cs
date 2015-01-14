using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour
{		
	public float fadeSpeed = 2f;
	public float lowIntensity = 0.5f;
	public float highIntensity = 2f;
	public float changeMargin = 0.2f;
	public bool alarmOn;
	float targetIntensity;

	void Awake ()
	{
		light.intensity = 0f;
		targetIntensity = highIntensity;
	}

	void Update ()
	{
		if (alarmOn)
		{
			FadeToTargetIntensity ();
			CheckForTargetIntensityToggle ();
		}
		else
		{
			FadeToZeroIntensity ();
		}
	}

	void FadeToTargetIntensity ()
	{
		light.intensity = Mathf.Lerp (light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
	}

	void FadeToZeroIntensity ()
	{
		light.intensity = Mathf.Lerp (light.intensity, 0f, fadeSpeed * Time.deltaTime);
	}

	void CheckForTargetIntensityToggle ()
	{
		if (LightIntensityWithinMarginOfTarget ())
		{
			ToggleTargetIntensity ();
		}
	}

	bool LightIntensityWithinMarginOfTarget ()
	{
		return Mathf.Abs (targetIntensity - light.intensity) < changeMargin;
	}

	void ToggleTargetIntensity ()
	{
		if (targetIntensity == highIntensity)
		{
			targetIntensity = lowIntensity;
		}
		else
		{
			targetIntensity = highIntensity;
		}
	}
}
