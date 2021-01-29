﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
	public GameObject DigSpotPrefab;
	public TopDownSteering CannonBallPrefab;
	public ParticleSystem CannonPrefab;
	public Seeker Shark;

	public int DigSpotsSqrRoot;
	public float DigSpotRandomRadius;
	public float DigSpotSpacing;

	public float CannonBallDistance;
	public float CannonBallHeight;
	public float CannonBallHeightVariance;
	public float CannonBallRate;
	public float CannonBallRateVariance;

	List<Vector3> _cannonBallSpawns;
	List<ParticleSystem> _cannons;
	float _nextSpawn;

	void Awake()
	{
		if (DigSpotPrefab == null) { Debug.Log("You forgot to set the dig spot prefab", gameObject); }
		if (CannonBallPrefab == null) { Debug.Log("You forgot to set the cannon ball prefab", gameObject); }
		if (Shark == null) { Debug.Log("You forgot to set the shark object", gameObject); }

		_nextSpawn = 0;
	}

    void Start()
    {
		var (digPositions, cannonPositions) = DigPositions(DigSpotsSqrRoot, DigSpotSpacing);
		foreach (var pos in digPositions)
		{
			Object.Instantiate(DigSpotPrefab, pos, Quaternion.identity);
		}

		_cannonBallSpawns = cannonPositions;
		_cannons = new List<ParticleSystem>();
		foreach (var pos in _cannonBallSpawns)
		{
			var goal = CannonBallGoal(pos);
			float yAngle = Angle.Degrees(goal);
			_cannons.Add(Object.Instantiate(CannonPrefab, pos, Quaternion.Euler(0, 90.0f - yAngle, 0)).GetComponent<ParticleSystem>());
		}

		_nextSpawn = NextCannonBallSpawnTime(Time.time);

		Shark.GetComponent<NavMeshAgent>().Warp(SharkStartPosition(Shark));
    }

    // Update is called once per frame
    void Update()
    {
		float now = Time.time;

		while (now >= _nextSpawn)
		{
			var (pos, goal, index) = CannonBallSpawn();

			var cannonBall = Object.Instantiate(CannonBallPrefab, pos, Quaternion.identity).GetComponent<TopDownSteering>();
			cannonBall.GoalDirection = goal;
			cannonBall.InitialFacingDegrees = -Angle.Degrees(goal);
			_cannons[index].Play();

			_nextSpawn = NextCannonBallSpawnTime(_nextSpawn);
		}
    }

	// get random cannon ball spawn position and goal direction
	(Vector3 pos, Vector2 goal, int index) CannonBallSpawn()
	{
		int spawn = Random.Range(0, _cannonBallSpawns.Count);

		var pos = _cannonBallSpawns[spawn];
		pos.y += (Random.value * 2.0f - 0.5f) * CannonBallHeightVariance;

		return (pos, CannonBallGoal(pos), spawn);
	}

	// return goal in top-down XY coords (Y is Z in 3D)
	Vector2 CannonBallGoal(Vector3 cannonBallStart)
	{
		bool topLeft = cannonBallStart.z > cannonBallStart.x;
		bool topRight = cannonBallStart.z > -cannonBallStart.x;

		if (topLeft && topRight)
		{
			return new Vector2(0, -1);
		}
		else if (topLeft && !topRight)
		{
			return new Vector2(1, 0);
		}
		else if (!topLeft && topRight)
		{
			return new Vector2(-1, 0);
		}
		else
		{
			return new Vector2(0, 1);
		}
	}

	float NextCannonBallSpawnTime(float previousTime)
	{
		return CannonBallRate + Random.value * CannonBallRateVariance + previousTime;
	}

	// construct list of dig positions, will all be at Y=0. Make cannon ball spawns align
	(List<Vector3> digs, List<Vector3> cannonBalls) DigPositions(int sqrRoot, float spacing)
	{
		float corner = ((sqrRoot - 1) * spacing) / -2.0f;

		var digPositions = new List<Vector3>();
		var cannonBallPositions = new List<Vector3>();

		for (int x = 0; x < sqrRoot; ++x)
		{
			cannonBallPositions.Add(new Vector3(corner + x * spacing, CannonBallHeight, CannonBallDistance - corner));
			cannonBallPositions.Add(new Vector3(corner + x * spacing, CannonBallHeight, corner - CannonBallDistance));
			cannonBallPositions.Add(new Vector3(CannonBallDistance - corner, CannonBallHeight, corner + x * spacing));
			cannonBallPositions.Add(new Vector3(corner - CannonBallDistance, CannonBallHeight, corner + x * spacing));

			for (int y = 0; y < sqrRoot; ++y)
			{
				var rand = Random.insideUnitCircle * DigSpotRandomRadius;
				digPositions.Add(new Vector3(corner + x * spacing + rand.x, 0.0f, corner + y * spacing + rand.y));
			}
		}

		return (digPositions, cannonBallPositions);
	}

	Vector3 SharkStartPosition(Seeker shark)
	{
		float angle = Random.value * Mathf.PI * 2;
		return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * shark.WanderRadius;
	}
}
