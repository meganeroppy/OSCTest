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
		Title,
		Starting,
		Ingame,
		Quitting,
	}
	private State state; 

	private long lastTimeStamp;

	void Awake()
	{
		lastTimeStamp = -1;

		UpdateState( State.Title ); 

		OSCHandler.Instance.InitServer();
	}

	float timer = 0;

	// Update is called once per frame
	void Update () 
	{
		if( state == State.Starting || state == State.Quitting )
		{
				
			// 遷移中はタイマーを更新
			timer -= Time.deltaTime;
			if( timer <= 0 )
			{
				var newState = state == State.Starting ? State.Ingame : State.Title;
				UpdateState(newState);
			}

			return;
		}

		CheckReceive( );
	}
		
	/// <summary>
	/// 受信していないかチェック
	/// </summary>
	private void CheckReceive()
	{
		// チェックするアドレスを指定する
		string checkAddress = state == State.Title ? "start" : "quit";

		// 受信を確認したあとのステートを指定する タイトル画面は開始中 ゲーム中は終了中
		var stateAfter = state == State.Title ? State.Starting : State.Quitting;

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
					if( address.Contains(checkAddress) )
					{
						byte result;
						if( byte.TryParse( item.Value.packets[i].Data[0].ToString(), out result ))
						{
							if( result == 1 )
							{
								UpdateState( stateAfter );
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

		switch( newState )
		{
		case State.Title:
			newText = "タイトル画面 命令を受信したらゲームを開始します";
			newPanelColor = Color.gray;
			break;
		case State.Starting:
			newText = "ゲームを開始しています";
			newPanelColor = Color.cyan;
			timer = 3f;
			break;
		case State.Ingame:
			newText = "ゲームプレイ中 中断命令を受信したらゲームを中断します";
			newPanelColor = Color.blue;
			break;
		case State.Quitting:
			default:
			newText = "ゲーム中断中";
			newPanelColor = Color.yellow;
			timer = 3f;
			break;
		}

		panelBg.color = newPanelColor;
		panelText.text = newText;
		state = newState;
	}
}