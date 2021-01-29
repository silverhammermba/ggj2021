using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	public ParticleSystem NormalParticles;
	public ParticleSystem MagicParticles;

	void Awake()
	{
		if (NormalParticles == null) { Debug.Log("no particle system for cannon", gameObject); }
		if (MagicParticles == null) { Debug.Log("no magic particle system for cannon", gameObject); }
	}

	public void Fire(bool magic)
	{
		if (magic)
		{
			MagicParticles.Play();
		}
		else
		{
			NormalParticles.Play();
		}
	}
}
