using System;

public class BossEncouterEventArgs : EventArgs {
	public int BossID { get; }
	public Enemy Boss { get; }
	public Player EncouteredBy { get; }

	public BossEncouterEventArgs(int bossID, Enemy boss, Player player) {
		BossID = bossID;
		Boss = boss;
		EncouteredBy = player;
	}
}