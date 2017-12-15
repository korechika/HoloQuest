using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uOSC;
using System;

public class SwordController : MonoBehaviour {

	public GameObject m_swordPrefab;
	private GameObject m_swordObj;

	/// <summary>
	/// 剣の位置を更新
	/// </summary>
	public void UpdateSwordTransform (Vector3 handPos, Vector3 handRot) {
		if (m_swordObj.activeSelf) {
			m_swordObj.transform.position = handPos;
			m_swordObj.transform.eulerAngles = handRot;
		}
	}
	public void UpdateSwordTransform (Quaternion quaternion) {
		if (m_swordObj.activeSelf) {
			m_swordObj.transform.rotation = quaternion;
		}
	}

	public void Activate (bool flg) {
		m_swordObj.SetActive (flg);
	}

	/// <summary>
	/// Instantiates the sword prefab.
	/// </summary>
	public Transform InstantiateSwordPrefab () {
		GameObject swordObj = Instantiate (m_swordPrefab);
		PlaceGameObject.PlaceObjectAtInfront (swordObj);
		m_swordObj = swordObj;
		return swordObj.transform;
	}
}
