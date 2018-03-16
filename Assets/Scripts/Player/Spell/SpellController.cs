using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour {
	
	// 呪文の種類
	public enum SpellType : int
	{
		FIRE = 0,
		ICE
	}

	public GameObject[] m_SpellPrefab;
	public GameObject m_SpellCirclePrefab;
	private GameObject m_currentSpell;
	private GameObject m_spellCircle;

	private int m_currentSpedllID = 0;
	private bool m_isShooting = false;

	/// <summary>
	/// 現在の呪文を発射（現在は攻撃呪文のみを扱うため敵モンスターに呪文が飛んでいく）
	/// TODO: いずれ回復呪文などの実装も視野に
	/// </summary>
	public void Shoot () {
		Debug.Log ("Shoot!");
		if (m_currentSpell) {
			ShootSpell ();
		} else {
			SetSpell (SpellType.FIRE);
			ShootSpell ();
		}
	}

	private void ShootSpell() {
		m_isShooting = true;
		if (GameObject.FindWithTag ("Monster")) {
			StartCoroutine (_MoveSpell (m_currentSpell, m_currentSpell.transform.position, GameObject.FindWithTag ("Monster").transform.position));
		} else {
			StartCoroutine (_MoveSpell (m_currentSpell, m_currentSpell.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 1.0f));
		}
	}

	/// <summary>
	/// 呪文の発射が終了したとき
	/// </summary>
	public void ShootEnd() {
		Debug.Log ("Shoot End");
		m_isShooting = false;
		Destroy (m_currentSpell);
	}

	public void UpdateSpellTransform (Vector3 handPos, Vector3 handRot) {
		if (m_currentSpell.activeSelf && !m_isShooting) {
			m_currentSpell.transform.position = handPos;
			m_currentSpell.transform.eulerAngles = handRot;
		}
	}

	public void UpdateSpellCircleTransform (Vector3 handPos, Vector3 handRot) {
		if (m_spellCircle) {
			m_spellCircle.transform.position = handPos;
			m_spellCircle.transform.eulerAngles = handRot;
		}
	}

	/// <summary>
	/// 呪文をセット
	/// </summary>
	public void SetSpell (SpellType type) {
		m_currentSpell = Instantiate (m_SpellPrefab [(int)type]);
		m_currentSpell.SetActive (true);
		m_currentSpell.transform.position = m_spellCircle.transform.position;
		m_currentSpell.transform.rotation = m_spellCircle.transform.rotation;
		m_currentSpedllID = (int)type;
	}

	/// <summary>
	/// 魔法陣を展開
	/// </summary>
	public void SetSpellCircle () {
		m_spellCircle = Instantiate (m_SpellCirclePrefab);
	}

	/// <summary>
	/// 呪文を解除
	/// </summary>
	public void UnsetSpell() {
		Destroy (m_spellCircle);
	}

	/// <summary>
	/// 呪文が time 時間かけて startPos から endPos へ移動
	/// </summary>
	private IEnumerator _MoveSpell(GameObject spellObj, Vector3 startPos, Vector3 endPos, float time = 1.0f) {
		var moveHash = new Hashtable ();
		moveHash.Add ("position", endPos);
		moveHash.Add ("time", time);
		moveHash.Add ("oncompletetarget", this.gameObject);
		moveHash.Add ("oncomplete", "ShootEnd");
		moveHash.Add ("easeType", iTween.EaseType.easeInCubic);
		iTween.MoveTo (spellObj, moveHash);
		// 衝突判定も
		yield return null;
	}
}
