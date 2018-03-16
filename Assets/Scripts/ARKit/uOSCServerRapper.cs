using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

namespace uOSC
{
	/// <summary>
	/// Recieve messages from uOSCServer and invoke UnityEvent.
	/// </summary>
	[RequireComponent(typeof(uOscServer))]
	public class uOSCServerRapper : MonoBehaviour
	{
		public enum OpeType : int {
			EXIT = -1,
			START = 0,
			CALIBRATION,
			SPELL,
			MENU,
			OTHER,
			SPELL_LAUNCH,
			SPELL_CANCEL
		}

		[Serializable]
		public class ReceiveTransformEvent : UnityEvent <Vector3, Vector3> {}

		private uOscServer m_uOSCServer = null;
		private RightHand m_rightHand;
		private ReceiveTransformEvent m_onTransformReceived;

		void Awake()
		{
			m_onTransformReceived = new ReceiveTransformEvent ();
		}

		void Start()
		{
			m_uOSCServer = FindObjectOfType<uOscServer>();
			m_uOSCServer.onDataReceived.AddListener(_OnDataReceived);
			m_rightHand = FindObjectOfType <RightHand> ();
			if (!m_rightHand) {
				Debug.Log(this + ": no RightHand object could be found.");
			}
		}


		/// <summary>
		/// This function will be called when uOSCServer receive messages.
		/// </summary>
		/// <param name="message">transform of controller</param>
		private void _OnDataReceived (Message message)
		{
			Debug.Log ("address:" + message.address); 
			string[] msgs = message.values [0].GetString ().Split (' ');
			if (msgs.Length == 6) {
				Vector3 pos = _GetPositionFromMsg (msgs);
				Vector3 rot = _GetRotationFromMsg (msgs);
				m_onTransformReceived.Invoke (pos, rot);
			} else {
				int msg = int.Parse (msgs [0]);
				if (-1 <= msg && msg < Enum.GetValues (typeof(OpeType)).Length - 1) {
					_ParseOperationMessage (msg);
				} else {
					Debug.Log ("unknown message is " + msgs [0]);
				}
			}
		}   

		/// <summary>
		/// Parses the operation message.
		/// </summary>
		private void _ParseOperationMessage (int op) {
			switch (op) {
			case (int)OpeType.EXIT:
				SceneManager.LoadScene ("WorldMain");
				break;
			case (int)OpeType.START:
				// not use yet
				break;
			case (int)OpeType.CALIBRATION:
//				VuforiaController.Instance.EnableVuforia (true);
				break;
			case (int)OpeType.SPELL:
				m_rightHand.ChangeToSpellMode ();
				break;
			case (int)OpeType.MENU:
				// not use yet
				break;
			case (int)OpeType.OTHER:
				// not use yet
				break;
			case (int)OpeType.SPELL_LAUNCH:
				m_rightHand.SpellLaunch ();
				break;
			case (int)OpeType.SPELL_CANCEL:
				m_rightHand.SpellCancel ();
				break;
			default:
				break;
			}
		}

		/// <summary>
		///  split messages into position
		/// </summary>
		private Vector3 _GetPositionFromMsg (string[] msgs) {
			return new Vector3 (float.Parse(msgs [0]), float.Parse(msgs [1]), float.Parse(msgs [2]));
		}

		/// <summary>
		/// split messages into rotation
		/// </summary>
		private Vector3 _GetRotationFromMsg (string[] msgs) {
			return new Vector3 (float.Parse(msgs [3]), float.Parse(msgs [4]), float.Parse(msgs [5]));
		}

		/// <summary>
		/// Set the call back function
		/// </summary>
		public void SetTransformReceivedCallback (UnityAction<Vector3, Vector3> callback) {
			if (m_onTransformReceived != null) {
				m_onTransformReceived.AddListener (callback);
			} else {
				Debug.Log ("m_onTransformReceived is null");
			}
		}

		/// <summary>
		/// Set the call back function
		/// </summary>
		public void SetOperationReceivedCallback (UnityAction<Vector3, Vector3> callback) {
			if (m_onTransformReceived != null) {
				m_onTransformReceived.AddListener (callback);
			} else {
				Debug.Log ("m_onTransformReceived is null");
			}
		}
	}
}
