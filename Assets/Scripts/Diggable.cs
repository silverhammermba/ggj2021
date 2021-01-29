using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diggable : MonoBehaviour
{
	public float TotalWork;

	float _workRemaining;
	Digger _digger;

	void Awake()
	{
		_workRemaining = TotalWork;
		_digger = null;
	}

    void Start()
    {
    }

	public void StartDigging(Digger digger)
	{
		_digger = digger;
	}

	public void StopDigging()
	{
		_digger = null;
	}

    void Update()
    {
		if (_digger != null)
		{
			_workRemaining -= _digger.DigSpeed * Time.deltaTime;

			if (_workRemaining <= 0)
			{
				// TODO: give digger some reward
				Destroy(gameObject);
			}
		}
    }

	void OnTriggerEnter(Collider other)
	{
		var digger = other.GetComponent<Digger>();
		if (digger == null) return;

		digger.SetTarget(this);
	}

	void OnTriggerExit(Collider other)
	{
		var digger = other.GetComponent<Digger>();
		if (digger == null) return;

		digger.UnsetTarget(this);
	}
}
