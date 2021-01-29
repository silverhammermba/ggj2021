using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Seeker : MonoBehaviour
{
	public GameObject Target; // target to seek
	public BoxCollider SafeArea; // will not seek target while inside bounds
	public float SeekSpeed; // top speed when seeking
	public float WanderRadius; // how far away from origin we will wander
	public float WanderSpeed; // top speed when wandering
	public float WanderDuration; // minimum num secs spent wandering to destination
	public float WanderDurationVariance; // max number of additional seconds spent wandering toward destination
	public float WanderFinishedThreshold; // how close we get to the wander destination before we wander elsewhere

	float _nextWander; // time when we get a new wander target
	Vector3 _wanderPosition; // position we are wandering towards

	NavMeshAgent _agent;

	void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		if (_agent == null) { Debug.Log("no agent for seeker to use", gameObject); }
	}

	void Start()
	{
		_nextWander = 0;
		_wanderPosition = Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
		if (Target != null && !SafeArea.bounds.Contains(Target.transform.position))
		{
			_agent.speed = SeekSpeed;
			_agent.SetDestination(Target.transform.position);
		}
		else
		{
			_agent.speed = WanderSpeed;
			var wander = NextWanderPosition();
			_agent.SetDestination(wander);
		}
    }

	Vector3 NextWanderPosition()
	{
		float now = Time.time;
		if (now >= _nextWander || Vector3.Distance(_agent.destination, transform.position) < WanderFinishedThreshold)
		{
			_nextWander = NextWanderTime(now);
			_wanderPosition = ValidWanderPosition();
		}
		return _wanderPosition;
	}

	Vector3 ValidWanderPosition()
	{
		Vector3 wander;
		do
		{
			var newWander = Random.insideUnitCircle * WanderRadius;
			wander = new Vector3(newWander.x, 0, newWander.y);
		}
		while (SafeArea.bounds.Contains(wander));

		return wander;
	}

	float NextWanderTime(float previousWander)
	{
		return WanderDuration + previousWander + Random.value * WanderDurationVariance;
	}
}
