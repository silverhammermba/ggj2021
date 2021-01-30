using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diggable : MonoBehaviour
{
	public float TotalWork;
	public float DiggerShoveStrength;
	public float TreasureThrowStrength;

	public GameController Controller;
	public Treasure ContentsPrefab;
	public int Quantity;

	float _workDone;
	Digger _digger;
	Canvas _digUI;
	Image _progressImage;

	void Awake()
	{
		_digUI = GetComponentInChildren<Canvas>();
		if (_digUI == null) { Debug.Log("no canvas component in diggable children", gameObject); }

		_progressImage = _digUI.GetComponentInChildren<Image>();
		if (_progressImage == null) { Debug.Log("no image component in diggable canvas children", gameObject); }

		_workDone = 0;
		_digger = null;
	}

	void Start()
	{
		if (Controller == null) { Debug.Log("no manager to track this diggable!", gameObject); }
	}

	public void StartDigging(Digger digger)
	{
		_digger = digger;
		_digUI.enabled = true;
	}

	public void StopDigging()
	{
		_digger = null;
	}

    void Update()
    {
		if (_digger != null)
		{
			_workDone += _digger.DigSpeed * Time.deltaTime;

			if (_workDone >= TotalWork)
			{
				FinishDigging();
			}
			else
			{
				_progressImage.fillAmount = 1.0f - _workDone / TotalWork;
			}
		}
    }

	void FinishDigging()
	{
		var rigidbody = _digger.GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			float fixedUpdatesPerSec = 1 / Time.fixedDeltaTime;
			float dragCompensation = fixedUpdatesPerSec / Mathf.Max(fixedUpdatesPerSec - rigidbody.drag, Mathf.Epsilon);
			float jumpImpulse = DiggerShoveStrength * Mathf.Sqrt(-Physics.gravity.y) * dragCompensation;

			rigidbody.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
		}

		_digger.OutOfRange(this);

		StartCoroutine(LaunchTreasure(Quantity, transform.position, ContentsPrefab, Controller, TreasureThrowStrength));
	}

	IEnumerator LaunchTreasure(int quantity, Vector3 start, Treasure prefab, GameController controller, float strength)
	{
		for (var i = 0; i < quantity; ++i)
		{
			var treasure = Object.Instantiate(prefab, start, Quaternion.identity).GetComponent<Treasure>();
			float randRadians = Random.value * Mathf.PI * 2;
			treasure.Controller = controller;

			var rigidbody = treasure.GetComponent<Rigidbody>();
			if (rigidbody != null)
			{
				rigidbody.AddForce(new Vector3(Mathf.Cos(randRadians), 1.0f, Mathf.Sin(randRadians)) * strength, ForceMode.Impulse);
			}
			yield return null;
		}

		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		var digger = other.GetComponent<Digger>();
		if (digger == null) return;

		digger.InRange(this);
	}

	void OnTriggerExit(Collider other)
	{
		var digger = other.GetComponent<Digger>();
		if (digger == null) return;

		digger.OutOfRange(this);
		_digUI.enabled = false;
	}
}
