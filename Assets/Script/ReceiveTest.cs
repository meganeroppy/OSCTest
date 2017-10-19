using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

/// <summary>
/// 受信テスト
/// </summary>
public class ReceiveTest : MonoBehaviour 
{
	enum State{
		Wait,
		Ingame,
	}
	private State state; 

	private long lastTimeStamp;

	/// <summary>
	/// ローカルのゲーム開始フラグ
	/// </summary>
	private byte startFlag = 0;

	/// <summary>
	/// ローカルのゲーム中断フラグ
	/// </summary>
	private byte quitFlag = 0;

	void Awake()
	{
		startFlag = 0;
		quitFlag = 0;

		lastTimeStamp = -1;

		UpdateState( State.Wait ); 

		OSCHandler.Instance.InitServer();
	}

	// Update is called once per frame
	void Update () 
	{
		string checkAddress = state == State.Wait ? "start" : "quit";
		var stateAfter = state == State.Wait ? State.Ingame : State.Wait;

		CheckReceive( checkAddress, () => UpdateState( stateAfter ) );
	}
		
	/// <summary>
	/// 受信していないかチェック
	/// </summary>
	private void CheckReceive( string chkAddress, System.Action action )
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

					// 適切なアドレスが来ていたら0番目の値でフラグを更新
					if( address.Contains(chkAddress) )
					{
						byte result;
						if( byte.TryParse( item.Value.packets[i].Data[0].ToString(), out result ))
						{
							if( result == 1 )
							{
								action();
							}
						}
					}
				}
			}
		}	
	}

	[SerializeField]
	private Image panelBg;

	[SerializeField]
	private Text panelText;

	/// <summary>
	/// 画面表示を更新
	/// </summary>
	private void UpdateState( State newState )
	{
		string newText;
		Color newPanelColor;

		if( newState == State.Ingame )
		{
			newText = "ゲームプレイ中";
			newPanelColor = Color.blue;
		}
		else
		{
			newText = "開始待機中";
			newPanelColor = Color.yellow;
		}

		panelBg.color = newPanelColor;
		panelText.text = newText;
		state = newState;
	}
}