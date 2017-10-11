//Data to be saved
[System.Serializable]
public class SaveData {

	public int localAttempt;

	public int coinsCollected;
	public int spikesCollected;
	public int bullets;
	public int bombs;

	public float playerPositionX, playerPositionY, playerPositionZ;
	public float blockPosX, blockPosY, blockPosZ;
	public float spikePosX, spikePosY, spikePosZ;
	public float blockZRotation;
	public bool spikeActive;

	public int difficulty;
	public float time;
	public bool canZoom;

	public string currentBGName;
	public string currentlyDisplayedSideInfo;

	public bool shownShotInfo;
	public bool shownAttempt;
	public bool shownBlockInfo;
	public int blockPushAttempt;
	public bool pressurePlateTriggered;
	public bool postMazeDoorOpen;

	public float camSize;

	public bool isNewGame;
	public bool isRestarting;


	public bool shownAvoidanceInfo;
	public bool doneAvoidance;
	public bool bossSpawned;

}

