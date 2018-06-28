using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameplayManagerMP : NetworkBehaviour {
	[SyncVar(hook = "OnValChanged")]
	public int state = 0;

	private void Start() {
		NetworkServer.RegisterHandler(MsgType.Connect, PlayerConnected);
		if(!isClient){
			gameObject.name = "Server";
		}
		else{
			gameObject.name = "Client";
		}
	}


	public override void OnStartLocalPlayer() {
		base.OnStartLocalPlayer();
	}

	public void SetValue() {
		if (isClient) {
			try {
				print(GameObject.Find("Player(Clone)").name);
				print(localPlayerAuthority);
				GameObject.Find("Client").GetComponent<GameplayManagerMP>().CmdTest();
			}
			catch (System.Exception e) {
				GameObject.Find("_CurrentField").GetComponent<Text>().color = Color.red;
			}
		}
		if(state == 0) {
			state = 20;
		}
		else {
			state = 0;
		}
	}

	public void OnValChanged(int val) {
		print("Val changed");
		GameObject.Find("_CurrentField").GetComponent<Text>().color = Random.ColorHSV();
		if (isClient) {
			CmdTestSuccessful(val);
		}
	}

	[Command]
	public void CmdTest() {
		print("Pressed that button!");
	}

	[ClientRpc]
	public void RpcTest(string s) {
		GameObject.Find("_CurrentField").GetComponent<Text>().color = Random.ColorHSV();
		CmdTestSuccessful(10);
	}

	[ClientRpc]
	public void RpcName(string s) {
		gameObject.name = s;
	}

	[Command]
	public void CmdTestSuccessful(int i) {
		if(i == 0) {
			GameObject.Find("_CurrentField").GetComponent<Text>().color = Color.black;
		}
		else if(i == 20) {
			GameObject.Find("_CurrentField").GetComponent<Text>().color = Color.blue;
		}
		else if(i == 10) {
			GameObject.Find("_CurrentField").GetComponent<Text>().color = Color.yellow;
		}
		print("Test was successful," + i);
	}

	private void PlayerConnected(NetworkMessage message) {
		
		print("This thing happend " + message.msgType);
		RpcTest("Who Are you!");
	}
}
