using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityOSC;

/// <summary>
/// 送信シーンの制御
/// </summary>
public class SendSceneController : MonoBehaviour 
{
	[SerializeField]
	string[] presetIpAddress = new string[]{"127.0.0.1"};

	[SerializeField]
	UnityEngine.UI.InputField[] inputField;

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