using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VuforiaController : SingletonMonoBehaviour<VuforiaController> {

	void Start() {
		EnableVuforia (false);
	}

	/// <summary>
	/// Enables the vuforia.
	/// </summary>
	public void EnableVuforia (bool enable) {
		if (VuforiaManager.Instance != null) {
			VuforiaBehaviour.Instance.enabled = enable;
			Debug.Log ("Vuforia:" + VuforiaBehaviour.Instance.isActiveAndEnabled);
		} else {
			Debug.Log ("No VuforiaManager instance found");
		}
	}
}
