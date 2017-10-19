using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityOSC;

/// <summary>
/// 送信シーンの制御
/// </summary>
public class SendSceneController : MonoBehaviour {

	private long lastTimeStamp;

	[SerializeField]
	string[] presetIpAddress = new string[]{"127.0.0.1"};

	[SerializeField]
	UnityEngine.UI.InputField[] inputField;

	private const string ClientId = "Yggdra";
	/*
	/// <summary>
	/// ゲーム開始フラグ
	/// </summary>
	private bool startFlag = false;

	/// <summary>
	/// 前回送信したゲーム開始フラグ
	/// </summary>
	private bool startFlagPrev = false;

	/// <summary>
	/// ゲーム強制終了フラグ
	/// </summary>
	private bool quitFlag = false;

	/// <summary>
	/// 前回送信したゲーム強制終了
	/// </summary>
	private bool quitFlagPrev = false;
	*/

	public static Dictionary<string, List<object>> values = new Dictionary<string, List<object>>();

	void Awake()
	{
			for( int i=0 ; i < inputField.Length ; ++i )
			{
				if( presetIpAddress.Length > i && !string.IsNullOrEmpty(presetIpAddress[i]))
				{
					inputField[i].text = presetIpAddress[i];

					OSCHandler.Instance.InitClient( presetIpAddress[i] );
				}
				else
				{
					inputField[i].text = "";
				}

				// インプットフィールドの値が変更されたときのイベントをセット
				inputField[i].onEndEdit.AddListener( str => {
					OSCHandler.Instance.InitClient(str);
				});
			}
	}
		
	/*
	// Update is called once per frame
	void Update () 
	{
		if( startFlag != startFlagPrev )
		{
			Send();
			valuesPrev = transform.position;
		}
	}
	*/

	/// <summary>
	/// ボタンが押された
	/// </summary>
	public void PressButton( string address )
	{
		Debug.Log(System.Reflection.MethodBase.GetCurrentMethod() + address);
		Send( address );
	}
		
	void Send(string address)
	{
		//  値の送信
		foreach( KeyValuePair<string, ClientLog> client in OSCHandler.Instance.Clients )
		{
			OSCHandler.Instance.SendMessageToClient(client.Key, address, 1 );
		}

		/*
		//  複合データの場合は
		List<object> values = new List<object>();
		values.AddRange(new object[]{transform.position.x, 
			transform.position.y, 
			transform.position.z });

		foreach( KeyValuePair<string, ClientLog> client in OSCHandler.Instance.Clients )
		{
			OSCHandler.Instance.SendMessageToClient(client.Key, "/positionXYZ", values );
		}
		*/
	}
}