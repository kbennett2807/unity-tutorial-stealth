using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour
{
	public Vector3 resetPosition = new Vector3 (1000f, 1000f, 1000f) ;
	public Vector3 position = new Vector3 (1000f, 1000f, 1000f);
	public float lightHighIntensity = 0.25f;
	public float lightLowIntensity = 0f;
	public float fadeSpeed = 7f;
	public float musicFadeSpeed = 1f;
	AlarmLight alarm;
	Light mainLight;
	AudioSource panicAudio;
	AudioSource[] sirens;

	void Awake ()
	{
		alarm = GameObject.FindWithTag (Tags.alarmLight).GetComponent<AlarmLight> ();
		mainLight = GameObject.FindWithTag (Tags.mainLight).light;
		panicAudio = transform.Find ("secondaryMusic").audio;
		GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag (Tags.siren);
		sirens = new AudioSource[sirenGameObjects.Length];

		for (int i = 0; i < sirens.Length; i++)
		{
			sirens [i] = sirenGameObjects [i].audio;
		}
	}

	void Update()
	{
		SwitchAlarms();
	}

	void SwitchAlarms ()
	{
		alarm.alarmOn = PlayerLocationKnown ();
		CheckMainLightIntensity ();
		CheckAlarmSounds ();
	}

	void CheckMainLightIntensity ()
	{
		float newIntensity;
		if (PlayerLocationKnown ())
		{
			newIntensity = lightLowIntensity;
		}
		else
		{
			newIntensity = lightHighIntensity;
		}
		mainLight.intensity = Mathf.Lerp (mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);
	}

	void CheckAlarmSounds ()
	{
		foreach (AudioSource siren in sirens)
		{
			if (PlayerLocationKnown () && !siren.audio.isPlaying)
			{
				siren.audio.Play ();
			}
			else if (!PlayerLocationKnown ())
			{
				siren.Stop ();
			}
		}
	}

	bool PlayerLocationKnown ()
	{
		return (position != resetPosition);
	}

	void CheckMusicFading()
	{
		if(PlayerLocationKnown())
		{
			audio.volume = Mathf.Lerp(audio.volume, 0f, musicFadeSpeed * Time.deltaTime);
			panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
		}
		else
		{
			audio.volume = Mathf.Lerp(audio.volume, 1f, musicFadeSpeed * Time.deltaTime);
			panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
		}
	}
}
