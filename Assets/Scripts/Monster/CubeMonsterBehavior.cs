using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CubeMonsterBehavior : Monster {

	public int m_HitPoint = 5;
	public int m_AttackPoint = 1;

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
		if (collision.gameObject.tag == "Army") {
			_ReduceHP (1);
		} else if (collision.gameObject.tag == "Spell") {
			_ReduceHP (1);
		} else if (collision.gameObject.tag == "UnderPlane") {
			StartCoroutine (ExtinguishMonster ());
			this.OnMonsterDisappeared.Invoke ();
		}
	}


	// モンスターのHPを減らす
	private void _ReduceHP (int damage) {
		if (this.hitPoint > 0) {
			this.hitPoint -= damage;
			this.UpdateHitPointText (m_hitPointText);
			this.transform.LookAt (Camera.main.transform);
		}
		if (hitPoint == 0) {
			hitPoint -= damage;
			StartCoroutine (ExtinguishMonster ());
			this.OnMonsterDisappeared.Invoke ();
		}
	}

	// プレイヤーへ攻撃
	protected override void AttackToPlayer () {
		
	}
}
