using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int m_playerHP = 10;

	/// <summary>
	/// Raises the collision enter event.
	/// </summary>
	void OnCollisionEnter(Collision collision) {
		Debug.Log ("player collision");
		if (collision.gameObject.tag == "Monster") {
			_ReduceHP (1);
		}
	}

	private void _ReduceHP(int count) {
		m_playerHP -= count;
		if (m_playerHP <= 0) {
			Debug.Log ("Game Over");
		}
	}
}
