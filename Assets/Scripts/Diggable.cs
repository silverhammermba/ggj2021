using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diggable : MonoBehaviour
{
	public float TotalWork;

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
				// TODO: give digger some reward
				Destroy(gameObject);
			}
			else
			{
				_progressImage.fillAmount = _workDone / TotalWork;
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
		_digUI.enabled = false;
	}
}
