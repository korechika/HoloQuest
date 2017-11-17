using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour {

	public enum SpellType : int
	{
		FIRE = 0,
		ICE
	}

	public GameObject[] m_SpellPrefab;
	private GameObject m_currentSpell;

	private int m_currentSpedllID = 0;
	private bool m_isShooting = false;

	public void Shoot () {
		Debug.Log ("Shoot!");
		if (m_currentSpell) {
			m_isShooting = true;
			if (GameObject.FindWithTag ("Monster")) {
				StartCoroutine (_MoveSpell (m_currentSpell, m_currentSpell.transform.position, GameObject.FindWithTag ("Monster").transform.position));
			} else {
				StartCoroutine (_MoveSpell (m_currentSpell, m_currentSpell.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 1.0f));
			}
		}
	}


	public void ShootEnd() {
		Debug.Log ("Shoot End");
		m_isShooting = false;
	}

	public void UpdateSpellTransform (Vector3 handPos, Vector3 handRot) {
		if (m_currentSpell.activeSelf && !m_isShooting) {
			m_currentSpell.transform.position = handPos;
			m_currentSpell.transform.eulerAngles = handRot;
		}
	}

	/// <summary>
	/// 呪文をセット
	/// </summary>
	public void SetSpell (SpellType type) {
		m_currentSpell = Instantiate (m_SpellPrefab [(int)type]);
		m_currentSpell.SetActive (true);
		m_currentSpedllID = (int)type;
	}

	/// <summary>
	/// 呪文を解除
	/// </summary>
	public void UnsetSpell() {
		m_currentSpell.SetActive (false);
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
		iTween.MoveTo (spellObj, moveHash);
		// 衝突判定も
		yield return null;
	}
}
