using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class Profile : MonoBehaviour {

	private static string profilesFolder = Application.persistentDataPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar;
	private static Profile_Data _currProfile;

	public static Profile_Data getCurrentProfile {
		get { return _currProfile; }
	}

	public static Profile_Data[] createdProfiles {
		get {
			DirectoryInfo d = new DirectoryInfo(profilesFolder);
			BinaryFormatter bf = new BinaryFormatter();
			FileInfo[] profilesInFolder = d.GetFiles("*.gp");
			Profile_Data[] allProfiles = new Profile_Data[profilesInFolder.Length];

			for (int i = 0; i < profilesInFolder.Length; i++) {
				using (FileStream fs = profilesInFolder[i].OpenRead()) {
					Profile_Data pd = (Profile_Data)bf.Deserialize(fs);
					allProfiles[i] = pd;
				}
			}
			return allProfiles;
		}
	}

	public static Profile_Data Create(string profileName) {
		BinaryFormatter bf = new BinaryFormatter();
		using (FileStream fs = File.Create(profilesFolder + profileName + ".gp")) {
			Profile_Data data = new Profile_Data(profileName);
			bf.Serialize(fs, data);
			return data;
		}
	}

	public static Profile_Data SelectProfile(GameObject profileObj) {
		BinaryFormatter bf = new BinaryFormatter();
		string profileName = profileObj.name;
		using (FileStream fs =File.OpenRead(profilesFolder + profileName)) {
			Profile_Data data = (Profile_Data)bf.Deserialize(fs);
			_currProfile = data;
			return data;
		}
	}
}

public class Profile_Data {
	private string _profileName = "";

	public Profile_Data(string name) {
		_profileName = name;
	}

	public string getProfileName {
		get { return _profileName; }
	}
}
