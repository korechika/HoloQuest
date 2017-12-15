using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

	public GameObject m_monsterPrefab;

	void Start ()
	{
		#if UNITY_EDITOR
		StartCoroutine (_SpawnMonsterAfterFieldSet(1f));
		#else
		StartCoroutine (_SpawnMonsterAfterFieldSet(5f));
		#endif
	}


	/// <summary>
	/// spawn the monster at designed position
	/// </summary>
	public GameObject SpawnMonsterPrefab (GameObject monsterPrefab, Vector3 pos) {
		GameObject monster = Instantiate (monsterPrefab);
		monster.transform.position = pos;
		this.LookAtCamera (monster);
		return monster;
	}


	/// <summary>
	/// Spawns the monster after field set.
	/// </summary>
	/// <returns>The monster after field set.</returns>
	/// <param name="waittime">Waittime.</param>
	private IEnumerator _SpawnMonsterAfterFieldSet (float waittime) {
		#if UNITY_EDITOR
		// distance between pop position and camera position
		float spawnDistance = 1f;
		#else 
		// distance between pop position and camera position
		float spawnDistance = 3f;
		#endif

		yield return new WaitForSeconds (waittime);
		SpawnMonsterPrefab (m_monsterPrefab, Camera.main.transform.position + Camera.main.transform.forward * spawnDistance);
		yield return null;
	}


	/// <summary>
	/// Object looks at camera.
	/// </summary>
	private void LookAtCamera (GameObject obj) {
		obj.transform.LookAt (Camera.main.transform);
	}


	/// <summary>
	/// Raises the all monster disappear event.
	/// </summary>
	public void OnAllMonsterDisappear () {
		StartCoroutine (_SpawnMonsterAfterFieldSet(1f));
	}
}
