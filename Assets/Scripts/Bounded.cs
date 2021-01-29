using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounded : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "wall")
		{
			Destroy(gameObject);
		}
	}
}
