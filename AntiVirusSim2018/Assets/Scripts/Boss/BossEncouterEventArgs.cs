using System;

public class BossEncouterEventArgs : EventArgs {
	public Enemy boss { get; }
	public M_Player encouteredBy { get; }

	public BossEncouterEventArgs(Enemy boss, M_Player player) {
		this.boss = boss;
		this.encouteredBy = player;
	}
}