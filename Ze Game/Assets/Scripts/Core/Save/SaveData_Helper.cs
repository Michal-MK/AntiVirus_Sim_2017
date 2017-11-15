using UnityEngine;

namespace SaveData_Helper {
	[System.Serializable]
	public class SVector3 {
		float x, y, z;
		public SVector3(float _x, float _y, float _z) {
			x = _x;
			y = _y;
			z = _z;
		}
		public static implicit operator Vector3(SVector3 vec) {
			return new Vector3(vec.x, vec.y, vec.z);
		}
		public static implicit operator SVector3(Vector3 vec) {
			return new SVector3(vec.x, vec.y, vec.z);
		}
	}

	[System.Serializable]
	public class DisplayedHints {
		public string currentlyDisplayedSideInfo;

		public bool shownShotInfo;
		public bool shownAttempt;
		public bool shownBlockInfo;
		public bool shownAvoidanceInfo;
	}

	[System.Serializable]
	public class Player {
		public SVector3 playerPos;
		public int coinsCollected;
		public int spikesCollected;
		public int bullets;
		public int bombs;

		public bool canZoom;
		public string currentBGName;
	}

	[System.Serializable]
	public class Core {
		public int localAttempt;
		public int difficulty;
		public float time;
		public float camSize;
		public string fileLocation;
		public string imgFileLocation;
	}

	[System.Serializable]
	public class World {
		public SVector3 blockPos;
		public float blockZRotation;

		public SVector3 spikePos;

		public bool spikeActive;

		public bool doneAvoidance;
		public bool bossSpawned;

		public int blockPushAttempt;
		public bool pressurePlateTriggered;
		public bool postMazeDoorOpen;
	}

	[System.Serializable]
	public class Maze {

	}
}

