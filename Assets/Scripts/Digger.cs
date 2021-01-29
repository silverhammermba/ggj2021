using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
	public float DigSpeed;

	Diggable _target; // what we are currently digging
	HashSet<Diggable> _inRange; // all things within digging range
	Rigidbody _rigidbody; // TODO: unused, maybe we want to stop moving while digging?

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		if (_rigidbody == null) { Debug.Log("no rigidbody for digger", gameObject); }

		_inRange = new HashSet<Diggable>();
	}

	public bool StartDigging()
	{
		StopDigging();

		Diggable closest = null;
		float closestDist = Mathf.Infinity;
		foreach (var target in _inRange)
		{
			float newDist = Vector3.Distance(transform.position, target.transform.position);
			if (newDist < closestDist)
			{
				closest = target;
				closestDist = newDist;
			}
		}

		if (closest != null)
		{
			_target = closest;
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
		_target = null;
	}

	public void InRange(Diggable target)
	{
		_inRange.Add(target);
	}

	public void OutOfRange(Diggable target)
	{
		_inRange.Remove(target);

		if (_target == target)
		{
			StopDigging();
		}
	}
}
