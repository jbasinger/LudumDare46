using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public AudioClip[] nameClips;
	public AudioClip[] foodClips;
	public AudioClip[] healClips;
	public AudioClip[] movementClips;
	public AudioClip[] hitClips;
	public AudioClip[] deadClips;
	public AudioClip[] noMoneyClips;
	public AudioClip trapClip;
	public AudioClip drawerClip;
	public AudioClip chachingClip;
	public AudioClip spawnerClip;

	AudioSource sound;
	Queue<int> queueClip = new Queue<int>();

	public static SoundManager GetManager()
	{
		return GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}

	private void Awake()
	{
		sound = GetComponent<AudioSource>();
	}

	public void PlaySpawnerClip()
	{
		sound.PlayOneShot(spawnerClip);
	}

	public void PlayTrapClip()
	{
		sound.PlayOneShot(trapClip);
	}

	public void PlayDrawerClip()
	{
		sound.PlayOneShot(drawerClip);
	}

	public void PlayChachingClip()
	{
		//This is just annoying.
		//sound.PlayOneShot(chachingClip);
	}

	public void QueueNameClip(int index)
	{
		queueClip.Enqueue(index);
	}

	public void PlayNameClip(int index)
	{
		sound.PlayOneShot(nameClips[index]);
	}

	public void PlayRandomDeadClip()
	{
		PlayRandomClip(deadClips,true);
	}

	public void PlayRandomNoMoneyClip()
	{
		PlayRandomClip(noMoneyClips,true);
	}

	public void PlayRandomFoodClip()
	{
		PlayRandomClip(foodClips, true);
	}

	public void PlayRandomHealClip()
	{
		PlayRandomClip(healClips, true);
	}

	public void PlayRandomHitClip()
	{
		PlayRandomClip(hitClips, true);
	}

	public void PlayRandomMovementClip()
	{
		PlayRandomClip(movementClips);
	}

	void PlayRandomClip(AudioClip[] clips, bool force = false)
	{

		if (sound.isPlaying && !force)
		{
			return;
		}

		if (clips.Length == 0)
		{
			Debug.LogError("Trying to play an empty list of sound clips randomly.");
			return;
		}

		AudioClip clip;

		if (clips.Length == 1)
		{
			clip = clips[0];
		}
		else
		{
			clip = clips[Random.Range(0, clips.Length)];
		}

		sound.PlayOneShot(clip);

	}

	private void Update()
	{
		if (queueClip.Count > 0 && !sound.isPlaying)
		{
			PlayNameClip(queueClip.Dequeue());
		}
	}

}
