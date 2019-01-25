using System;

public class BossfightResultEventArgs : EventArgs {
	public bool successfulKill { get; }

	public BossfightResultEventArgs(bool isSuccess) {
		successfulKill = isSuccess;
	}
}