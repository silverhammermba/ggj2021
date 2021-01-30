using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
	public GameController Controller;
	public Collider Trigger;
	public Collider PhysCollider;

	public float PickupDelay;
	public float MinThrowStrength;
	public float ThrowVariance;

	float _startTime;
	Rigidbody _rigidbody;
	bool _flung;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		if (_rigidbody == null) { Debug.Log("can't find rigidbody on treasure", gameObject); }
		if (Trigger == null) { Debug.Log("you forgot to set the trigger on treasure", gameObject); }
		if (PhysCollider == null) { Debug.Log("you forgot to set the phys collider on treasure", gameObject); }

		Trigger.enabled = false;
		PhysCollider.enabled = false;
		_flung = false;
	}

    void Start()
    {
		if (Controller == null) { Debug.Log("no manager to track this treasure!", gameObject); }

		_startTime = Time.time;
    }

    void Update()
    {
		if (!Trigger.enabled && Time.time - _startTime >= PickupDelay)
		{
			Trigger.enabled = true;
			PhysCollider.enabled = true;
		}
    }

	void FixedUpdate()
	{
		if (!_flung)
		{
			float randRadians = Random.value * Mathf.PI * 2;
			_rigidbody.AddForce(new Vector3(Mathf.Cos(randRadians), 2.0f, Mathf.Sin(randRadians)) * (MinThrowStrength + Random.value * ThrowVariance), ForceMode.Impulse);
			_flung = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (Controller != null) Controller.AddScore(1);
			Destroy(gameObject);
		}
	}
}
