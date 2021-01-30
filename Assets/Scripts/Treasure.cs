using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
	public GameController Controller;
	public float PickupDelay;
	public Collider Trigger;

	float _startTime;

    // Start is called before the first frame update
    void Start()
    {
		if (Controller == null) { Debug.Log("no manager to track this treasure!", gameObject); }

		_startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		if (!Trigger.enabled && Time.time - _startTime >= PickupDelay)
		{
			Trigger.enabled = true;
		}
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Controller.AddScore(1);
			Destroy(gameObject);
		}
	}
}
