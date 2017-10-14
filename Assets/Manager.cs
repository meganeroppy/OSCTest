using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class Manager : MonoBehaviour {

	private long lastTimeStamp;

	[SerializeField]
	OSCHandler.Mode mode;

	public static Dictionary<string, List<object>> values = new Dictionary<string, List<object>>();

	// Use this for initialization
	void Start () {
		//  OSC の初期化（受信開始）
		OSCHandler.Instance.Init(mode);

		if( mode.Equals( OSCHandler.Mode.Receive ) )
		{
			lastTimeStamp = -1;
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
		OSCHandler.Instance.SendMessageToClient("Yggdra", 
		                                          "/positionXYZ", values );
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
}