using UnityEngine;
using System.Collections.Generic;

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
		public bool shownBlockInfo;
		public bool shootingIntro;
	}

	[System.Serializable]
	public class Player {
		public SVector3 playerPos;
		public int coinsCollected;
		public int spikesCollected;
		public int bullets;
		public int bombs;
		public int gameProgression;
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

		public int blockPushAttempt;
		public bool pressurePlateTriggered;
		public bool postMazeDoorOpen;

		public bool boss1Killed;
		public List<string> doorsOpen = new List<string>();
	}

	[System.Serializable]
	public class Maze {

	}
}

