using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: need to make different ones for different death effects?
public class Killable : MonoBehaviour
{
	public void Die()
	{
		Destroy(gameObject);
	}
}
