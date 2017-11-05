using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeMonsterBehavior : Monster {

	public int m_HitPoint = 5;

	private GameObject HitPointTextObj;
	private TextMesh m_hitPointText;
	private MonsterSpawner m_monsterSpawner;

	void Start () {
		this.hitPoint = m_HitPoint;

		m_monsterSpawner = FindObjectOfType<MonsterSpawner> ();
		if (m_monsterSpawner != null) {
			this.OnMonsterDisappeared = new UnityEvent ();
			this.OnMonsterDisappeared.AddListener (m_monsterSpawner.OnAllMonsterDisappear);
		}

		this.HitPointTextObj = CreateHPTextObject ();

		m_hitPointText = this.HitPointTextObj.GetComponent<TextMesh> ();
		UpdateHitPointText (m_hitPointText);
	}
		

	void Update () {
		this.HPTextLookAtCamera (HitPointTextObj);
	}


	/// <summary>
	/// Raises the collision enter event.
	/// </summary>
	void OnCollisionEnter(Collision collision) {
		Debug.Log ("collision");
		if (collision.gameObject.tag == "Army") {
			if (this.hitPoint > 0) {
				this.hitPoint--;
				this.UpdateHitPointText (m_hitPointText);
				this.transform.LookAt (Camera.main.transform);
			}

			if (hitPoint == 0) {
				hitPoint--;
				StartCoroutine (ExtinguishMonster ());
				this.OnMonsterDisappeared.Invoke ();
			}
		}
	}
}
