using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uOSC;
using System;

public class SwordController : MonoBehaviour {

	public GameObject m_swordPrefab;
	private GameObject m_swordObj;
	private Vector3 m_swordOriginalPos = Vector3.zero;
	private Vector3 m_arKitOriginalPos = Vector3.zero;
	private Vector3 m_nowARKitPos = Vector3.zero;
	private ARKitTransformReceiver m_arKitTransformReceiver;
	private bool m_isFirstMessage = true;

	void Start () {
		m_swordObj = _InstantiateSwordPrefab ();

		m_arKitTransformReceiver = FindObjectOfType <ARKitTransformReceiver> ();
		if (m_arKitTransformReceiver == null) {
			Debug.LogError ("ARKitTransformReceiver not found");
		} else {
			m_arKitTransformReceiver.SetCallback (MoveSwordTransform);
		}
	}
		

	/// <summary>
	/// Moves the sword transform.
	/// </summary>
	/// <param name="arKitPos">Ar kit position from ARKit application.</param>
	/// <param name="arKitRot">Ar kit rotation from ARKit application.</param>
	public void MoveSwordTransform (Vector3 arKitPos, Vector3 arKitRot) {
		m_nowARKitPos = arKitPos;

		/*
		if (m_isFirstMessage) {
			m_swordObj.transform.Find ("Blade").gameObject.SetActive (true);
			m_isFirstMessage = false;
		}
		*/

		// update sword transform
		m_swordObj.transform.position = m_swordOriginalPos + (arKitPos - m_arKitOriginalPos);
		m_swordObj.transform.eulerAngles = arKitRot;
	}


	/// <summary>
	/// Updates the AR kit position.
	/// </summary>
	public void UpdateARKitPosition (Vector3 vuforiaPos) {
		m_swordOriginalPos = vuforiaPos;
		m_arKitOriginalPos = m_nowARKitPos;
	}


	/// <summary>
	/// Instantiates the sword prefab.
	/// </summary>
	/// <returns>The sword prefab.</returns>
	private GameObject _InstantiateSwordPrefab () {
		GameObject swordObj = Instantiate (m_swordPrefab);
		PlaceGameObject.PlaceObjectAtInfront (swordObj);
		m_swordOriginalPos = swordObj.transform.position;
		return swordObj;
	}
}
