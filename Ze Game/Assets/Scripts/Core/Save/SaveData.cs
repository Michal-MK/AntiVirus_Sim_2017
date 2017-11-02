using SaveData_Helper;
//Data to be saved
[System.Serializable]
public class SaveData {

	public int localAttempt;

	public int coinsCollected;
	public int spikesCollected;
	public int bullets;
	public int bombs;

	public SVector3 playerPos;

	public SVector3 blockPos;
	public float blockZRotation;

	public SVector3 spikePos;

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

	public Player player = new Player();
	public DisplayedHints shownHints = new DisplayedHints();
	public World world = new World();
	public Core core = new Core();

}





