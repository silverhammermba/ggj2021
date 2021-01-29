using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDig : MonoBehaviour
{
	Digger _digger;

	void Awake()
	{
		_digger = GetComponent<Digger>();
		if (_digger == null) { Debug.Log("inputdig has no digger", gameObject); }
	}

    void Update()
    {
		if (Input.GetButtonDown("Fire1"))
		{
			_digger.StartDigging();
		}
		else if (Input.GetButtonUp("Fire1"))
		{
			_digger.StopDigging();
		}
    }
}
