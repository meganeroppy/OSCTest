using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using info.shibuya24.osc;
using System.Net;

/// <summary>
/// 送信テストシーン制御
/// </summary>
public class SendSceneUIController : MonoBehaviour 
{
	// OSC送信インスタンス
	OSCSender m_sender;
	public Slider slider;

	[SerializeField]
	private string clientId = "TestClient";
	[SerializeField]
	private int port = 8890;
	[SerializeField]
	private string ipAddressStr = "192.168.3.1";


	void Start () 
	{
		m_sender = new OSCSender ();

		// 送信クライアント名のセット、IPアドレス、ポート番号は適宜調整
		// このIPアドレスとポート番号は受信側のIPアドレスとポート番号です
		//		m_sender.Init ("TestClient", 8890, IPAddress.Parse ("192.168.3." + Config.ipNum));
		IPAddress ipAddress;
		if( IPAddress.TryParse( ipAddressStr, out ipAddress ) )
		{
			m_sender.Init (clientId, port, ipAddress);
		}
		else
		{
			Debug.LogError("無効なIPアドレス -> [ " + ipAddressStr + " ]");
			return;
		}

		var e = new Slider.SliderEvent ();
		// Sendメソッドで送信
		e.AddListener (x => m_sender.Send ("/changeValue", x));
		slider.onValueChanged = e;	
	}

	public void OnClickButton(int id)
	{
		Debug.Log(id);
		m_sender.Send ("/changeValue", id);
	}
}
