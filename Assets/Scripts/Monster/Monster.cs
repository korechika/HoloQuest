using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Monster : MonoBehaviour{

	protected int hitPoint = 0;
	protected UnityEvent OnMonsterDisappeared;

	/// <summary>
	/// Extinguish the monster with particle effect
	/// </summary>
	protected IEnumerator ExtinguishMonster () {
		this.EffectDefeatParticle ();
		yield return new WaitForSeconds (2f);
		this.gameObject.SetActive (false);
	}


	/// <summary>
	/// Creates the HP text object.
	/// </summary>
	/// <returns>The HP text object.</returns>
	protected GameObject CreateHPTextObject () {
		GameObject obj = new GameObject ("HP");
		obj.AddComponent<TextMesh> ();

		// change scale of TextMesh to remove bleeding
		obj.GetComponent<TextMesh> ().fontSize = 50;
		obj.transform.localScale = obj.transform.localScale * 0.05f;

		HPTextLookAtCamera (obj);

		obj.transform.position = this.gameObject.transform.position + Vector3.up * 0.6f;

		// set TextMesh object under the monster object
		obj.transform.parent = this.transform;
		return obj;
	}


	/// <summary>
	/// Object looks at camera.
	/// </summary>
	protected void LookAtCamera (GameObject obj) {
		obj.transform.LookAt (Camera.main.transform);
	}


	/// <summary>
	/// HP Object looks at camera.
	/// </summary>
	protected void HPTextLookAtCamera (GameObject obj) {
		obj.transform.position = this.gameObject.transform.position + Vector3.up * 0.6f;
		LookAtCamera (obj);
		obj.transform.Rotate(Vector3.up - new Vector3(0, 180, 0));
	}


	/// <summary>
	/// Updates the hit point text.
	/// </summary>
	protected void UpdateHitPointText (TextMesh hpText) {
		hpText.text = "" + this.hitPoint;
	}


	/// <summary>
	/// Effects the defeat particle.
	/// </summary>
	protected void EffectDefeatParticle () {
		if (this.gameObject.GetComponent<ParticleSystem> () != null) {
			this.gameObject.GetComponent<ParticleSystem> ().Play ();
		}
	}

	protected abstract void AttackToPlayer ();
}
