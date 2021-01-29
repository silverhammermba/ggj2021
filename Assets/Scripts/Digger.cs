using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
	public float DigSpeed;

	Diggable _target;
	Rigidbody _rigidbody;
	bool _digging;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		if (_rigidbody == null) { Debug.Log("no rigidbody for digger", gameObject); }

		_digging = false;
	}

	public bool StartDigging()
	{
		if (_target != null)
		{
			if (_digging)
			{
				Debug.Log("somehow started digging while we already were?", gameObject);
				StopDigging();
			}
			_digging = true;
			_target.StartDigging(this);
			return true;
		}
		return false;
	}

	public void StopDigging()
	{
		if (_target != null)
		{
			_target.StopDigging();
		}
		_digging = false;
	}

	public void SetTarget(Diggable target)
	{
		StopDigging();
		_target = target;
	}

	public void UnsetTarget(Diggable target)
	{
		if (_target == target)
		{
			StopDigging();
			_target = null;
		}
	}
}
