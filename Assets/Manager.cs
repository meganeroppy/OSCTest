using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityOSC;

public class Manager : MonoBehaviour {

	private long lastTimeStamp;

	[SerializeField]
	OSCHandler.Mode mode;

	[SerializeField]
	string[] presetIpAddress = new string[]{"127.0.0.1"};

    [SerializeField]
    UnityEngine.UI.InputField[] inputField;

	public static Dictionary<string, List<object>> values = new Dictionary<string, List<object>>();

	void Awake()
	{
		if( mode.Equals( OSCHandler.Mode.Send ) )
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
		else
		{
			lastTimeStamp = -1;
			OSCHandler.Instance.InitServer();
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if( mode.Equals( OSCHandler.Mode.Send ) )
		{
			Send();
		}
		else
		{
			Receive();
		}
	}

	void Send()
	{
		//  単一データの送信
		//OSCHandler.Instance.SendMessageToClient("Yggdra", 
		//	"/positionX", 
		//	transform.position.x );

		//  複合データの場合は
		  List<object> values = new List<object>();
		  values.AddRange(new object[]{transform.position.x, 
		                               transform.position.y, 
		                               transform.position.z });

		foreach( KeyValuePair<string, ClientLog> client in OSCHandler.Instance.Clients )
		{
			OSCHandler.Instance.SendMessageToClient(client.Key, "/positionXYZ", values );
		}
	}

	void Receive()
	{
		//  受信データの更新
		OSCHandler.Instance.UpdateLogs();
		//  受信データの解析
		foreach (KeyValuePair<string, ServerLog> item in OSCHandler.Instance.Servers) {
			for (int i=0; i < item.Value.packets.Count; i++) {
				if (lastTimeStamp < item.Value.packets[i].TimeStamp) {
					lastTimeStamp = item.Value.packets[i].TimeStamp;
					//  アドレスパターン（文字列）
					string address = item.Value.packets[i].Address;
					//  引数（とりあえず最初の引数のみ）
					var arg0 = item.Value.packets[i].Data[0];
					var args = item.Value.packets[i].Data;
					//  処理（とりあえずコンソール出力）
					Debug.Log(address + ":" + arg0);

					UpdateValues( address, args );
				}
			}
		}
	}

	/// <summary>
	/// OSCクライアントから受け取った値を使ってメンバ変数の値を更新
	/// </summary>
	private void UpdateValues( string address, List<object> newValues )
	{
		if( !values.ContainsKey( address ) )
		{
			// キーが存在しなければ追加
			values.Add( address, newValues );
		}
		else
		{
			// キーが存在したら新しい値で上書き
			values[ address ] = newValues;
			}
	}

	public void SwitchScene()
	{
		if( mode.Equals( OSCHandler.Mode.Send ) )
		{
			SceneManager.LoadSceneAsync("Receive");
		}
		else
		{
			SceneManager.LoadSceneAsync("Send");
		}
	}
}