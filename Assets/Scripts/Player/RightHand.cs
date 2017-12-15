using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uOSC;

public class RightHand : MonoBehaviour {
	
	public enum AbilityType : int
	{
		SWORD = 0,
		SPELL
	}

	private SwordController m_swordController;
	private SpellController m_spellController;
	private uOSCServerRapper m_uOSCServerRapper;

	private AbilityType m_currentAbilityType = AbilityType.SWORD;

	private Vector3 m_handOriginalPos = Vector3.zero;		// handの基準となる位置、最初に設定
	private Vector3 m_arKitOriginalPos = Vector3.zero;		// ARKitが開始したときのコントローラの位置
	private Vector3 m_currentARKitPos = Vector3.zero;		// ARKitから受信した生のデータ

	// 角度はまだ微妙なので未使用
	private Vector3 m_handOriginalRot = Vector3.zero;
	private Vector3 m_arKitOriginalRot = Vector3.zero;
	private Vector3 m_currentARKitRot = Vector3.zero;

	void Start () {
		if (!Init ()) {
			return;
		}
		m_currentAbilityType = AbilityType.SWORD;
		Transform transform = m_swordController.InstantiateSwordPrefab ();	// 最初に剣を表示
		m_handOriginalPos = transform.position;
		m_handOriginalRot = transform.rotation.eulerAngles;
	}

	/// <summary>
	/// ARKit の Position と　Rotation をもとに右手の座標を決定し、アビリティの位置を更新
	/// </summary>
	private void _UpdateRightHandTransform (Vector3 arKitPos, Vector3 arKitRot) {
		m_currentARKitPos = arKitPos;
		m_currentARKitRot = arKitRot;
		Vector3 handPos = m_handOriginalPos + (arKitPos - m_arKitOriginalPos);
		Vector3 handRot = arKitRot;
		//Vector3 handRot = m_handOriginalRot + (arKitRot - m_arKitOriginalRot);
		_UpdateAbilityTransform (handPos, handRot);
	}


	private void _UpdateAbilityTransform (Vector3 handPos, Vector3 handRot) {
		switch (m_currentAbilityType) {
		case AbilityType.SWORD:
			m_swordController.UpdateSwordTransform (handPos, handRot);
			break;
		case AbilityType.SPELL:
			//m_spellController.UpdateSpellTransform (handPos, handRot);
			m_spellController.UpdateSpellCircleTransform (handPos, handRot);
			break;
		}
	}

	// アビリティをスペルモードに切り替え
	public void ChangeToSpellMode () {
		// TODO: vuforia
		Debug.Log("change to spell mode");
		SwitchAbility (AbilityType.SPELL);
	}

	// スペル発射
	public void SpellLaunch () {
		m_spellController.Shoot ();
		Debug.Log ("spell launch");
	}

	// スペルモードを解除
	public void SpellCancel () {
		SwitchAbility (AbilityType.SWORD);
		Debug.Log ("spell cancel");
	}

	/// <summary>
	/// Updates the AR kit transform.
	/// </summary>
	public void UpdateARKitTransform (Vector3 vuforiaPos, Vector3 vuforiaRot) {
		m_handOriginalPos = vuforiaPos;
		m_handOriginalRot = vuforiaRot;
		m_arKitOriginalPos = m_currentARKitPos;
		m_arKitOriginalRot = m_currentARKitRot;
		VuforiaController.Instance.EnableVuforia (false);	// 処理高速化のため、キャリブレーションしたらVuforiaを無効にする
	}

	// アビリティを変更する
	public void SwitchAbility (AbilityType type) {
		if (m_currentAbilityType != type) {
			_EndAbility (m_currentAbilityType);
			m_currentAbilityType = type;
			_StartAbility (m_currentAbilityType);
		} else {
			Debug.Log ("same ability type");
		}
	}

	// アビリティを終了
	private void _EndAbility (AbilityType type) {
		switch (type) {
		case AbilityType.SWORD:
			m_swordController.Activate (false);
			break;
		case AbilityType.SPELL:
			m_spellController.UnsetSpell ();
			break;
		default:
			break;
		}
	}

	// アビリティを開始
	private void _StartAbility (AbilityType type) {
		switch (type) {
		case AbilityType.SWORD:
			m_swordController.Activate (true);
			break;
		case AbilityType.SPELL:
			//m_spellController.SetSpell (SpellController.SpellType.FIRE);	// 魔法をセット
			m_spellController.SetSpellCircle ();	// 魔法陣をセット
			break;
		default:
			break;
		}
	}

	private bool Init() {
		bool success = true;
		m_swordController = FindObjectOfType<SwordController> ();
		if (!m_swordController) {
			Debug.Log ("No SwordController could be found.");
			success = false;
		}
		m_spellController = FindObjectOfType<SpellController> ();
		if (!m_spellController) {
			Debug.Log ("No SpellController could be found.");
			success = false;
		}

		m_uOSCServerRapper = FindObjectOfType <uOSCServerRapper> ();
		if (m_uOSCServerRapper == null) {
			Debug.LogError ("No ARKitTransformReceiver could be found.");
			success = false;
		} else {
			m_uOSCServerRapper.SetTransformReceivedCallback (_UpdateRightHandTransform);
		}
		return success;
	}
}
