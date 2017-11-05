using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class ReceiveTransformEvent : UnityEvent <Vector3, Vector3> {}

namespace uOSC
{
	/// <summary>
	/// Recieve messages from uOSCServer and invoke UnityEvent
	/// </summary>
	[RequireComponent(typeof(uOscServer))]
	public class ARKitTransformReceiver : MonoBehaviour
	{
		private uOscServer m_uOSCServer = null;
		private ReceiveTransformEvent m_onTransformReceived;

		void Awake()
		{
			m_onTransformReceived = new ReceiveTransformEvent ();
		}

		void Start()
		{
			m_uOSCServer = FindObjectOfType<uOscServer>();
			m_uOSCServer.onDataReceived.AddListener(_OnDataReceived);
		}


		/// <summary>
		/// This function will be called when uOSCServer receive messages.
		/// </summary>
		/// <param name="message">transform of controller</param>
		private void _OnDataReceived (Message message)
		{
			string[] msgs = message.values [0].GetString ().Split (' ');
			if (msgs.Length >= 6) {
				Vector3 pos = _GetPositionFromMsg (msgs);
				Vector3 rot = _GetRotationFromMsg (msgs);
				m_onTransformReceived.Invoke (pos, rot);
			} else {
				Debug.Log ("message is " + msgs);
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
		public void SetCallback (UnityAction<Vector3, Vector3> callback) {
			if (m_onTransformReceived != null) {
				m_onTransformReceived.AddListener (callback);
			} else {
				Debug.Log ("m_onTransformReceived is null");
			}
		}
	}
}
