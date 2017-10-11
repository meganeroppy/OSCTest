using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using info.shibuya24.osc;

/// <summary>
/// 受信テストシーン制御
/// </summary>
public class ReceiveSceneUIController : MonoBehaviour 
{
	// OSC受信インスタンス
	OSCReceiver m_receiver;

	[SerializeField]
	private UnityEngine.UI.Image image;

	[SerializeField]
	private string serverId = "TestServer";
	[SerializeField]
	private int port = 8890;

	void Start () 
	{
		m_receiver = new OSCReceiver();
//		m_receiver.Init ("TestServer", 8890);
		m_receiver.Init (serverId, port);
		// 受信インスタンスに受信した時のイベントを登録
		m_receiver.onListenToOSCMessage += OnListenToOSCMessage;
	}

	void OnListenToOSCMessage (UnityOSC.OSCPacket obj)
	{
		// 受信したときの処理
		Debug.Log( System.Reflection.MethodBase.GetCurrentMethod() + " : " );

		Debug.Log( string.Format("address:{0} binaryData:{1} Data:{2} TimeStamp:{3}", obj.Address.ToString(), obj.BinaryData, obj.Data.ToString(), obj.TimeStamp) );
	}
}
