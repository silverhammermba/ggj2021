using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusedKillable : ExplodingKillable
{
	public float FuseSeconds;
	public float FlashAmplitude;
	public float FlashSpread;
	public GameObject Visuals;

	float _startTime;

    // Start is called before the first frame update
    void Start()
    {
		_startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		float elapsed = Time.time - _startTime;
		if (elapsed >= FuseSeconds)
		{
			Die();
		}
		else
		{
			float remaining = FuseSeconds - elapsed;
			Flash(remaining);
		}
    }

	void Flash(float remaining)
	{
		// A sin(1/Sx^2)/x so fluctuations get faster and taller as remaining time decreases
		float amplitude = 1.0f + FlashAmplitude * Mathf.Sin(FlashSpread * remaining) / Mathf.Exp(remaining);
		Visuals.transform.localScale = new Vector3(amplitude, amplitude, amplitude);
	}
}
