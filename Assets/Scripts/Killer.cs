using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
	public bool SelfDestruct;

	void OnTriggerEnter(Collider other)
	{
		var killable = other.GetComponent<Killable>();
		if (killable == null) return;

		killable.Die();

		if (SelfDestruct)
		{
			killable = GetComponent<Killable>();
			if (killable == null)
			{
				Destroy(gameObject);
			}
			else
			{
				killable.Die();
			}
		}
	}
}
