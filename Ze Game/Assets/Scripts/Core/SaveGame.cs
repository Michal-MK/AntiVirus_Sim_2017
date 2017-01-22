using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour {

    public static SaveGame script;

    private void Awake() {
        script = this;
    }


    public void saveScore() {
        int difficulty = PlayerPrefs.GetInt("difficulty");

		if (difficulty == 0) {


			int q = 0;
			int count = 10;


			PlayerPrefs.SetFloat(count.ToString(), Mathf.Round(timer.time * 1000) / 1000);
            Debug.Log(PlayerPrefs.GetFloat("10"));
            while (q < count) {

                if (PlayerPrefs.HasKey((count - 1).ToString()) == true) {

                    if (PlayerPrefs.GetFloat((count - 1).ToString()) > PlayerPrefs.GetFloat(count.ToString())) {

                        float temp = PlayerPrefs.GetFloat((count - 1).ToString());
                        PlayerPrefs.SetFloat((count - 1).ToString(), PlayerPrefs.GetFloat(count.ToString()));
                        PlayerPrefs.SetFloat(count.ToString(), temp);
                    }
                    count -= 1;
                    PlayerPrefs.SetFloat("10", 500f);
                }
                else {
                    count = -1;
                }
            }
        }

        if (difficulty == 1) {

            int q = 11;
            int count = 21;


            PlayerPrefs.SetFloat(count.ToString(), Mathf.Round(timer.time * 1000) / 1000);

            while (q < count) {

                if (PlayerPrefs.HasKey((count - 1).ToString()) == true) {

                    if (PlayerPrefs.GetFloat((count - 1).ToString()) > PlayerPrefs.GetFloat(count.ToString())) {

                        float temp = PlayerPrefs.GetFloat((count - 1).ToString());
                        PlayerPrefs.SetFloat((count - 1).ToString(), PlayerPrefs.GetFloat(count.ToString()));
                        PlayerPrefs.SetFloat(count.ToString(), temp);
                    }
                    count -= 1;
                    PlayerPrefs.SetFloat("21", 500f);
                }
                else {
                    count = -1;
                }
            }

        }

        if (difficulty == 2) {

            int q = 22;
            int count = 32;


            PlayerPrefs.SetFloat(count.ToString(), Mathf.Round(timer.time * 1000) / 1000);

            while (q < count) {

                if (PlayerPrefs.HasKey((count - 1).ToString()) == true) {

                    if (PlayerPrefs.GetFloat((count - 1).ToString()) > PlayerPrefs.GetFloat(count.ToString())) {

                        float temp = PlayerPrefs.GetFloat((count - 1).ToString());
                        PlayerPrefs.SetFloat((count - 1).ToString(), PlayerPrefs.GetFloat(count.ToString()));
                        PlayerPrefs.SetFloat(count.ToString(), temp);
                    }
                    count -= 1;
                    PlayerPrefs.SetFloat("32", 500f);
                }
                else {
                    count = -1;
                }
            }

        }

        if (difficulty == 3) {

            int q = 33;
            int count = 43;


            PlayerPrefs.SetFloat(count.ToString(), Mathf.Round(timer.time * 1000) / 1000);

            while (q < count) {

                if (PlayerPrefs.HasKey((count - 1).ToString()) == true) {

                    if (PlayerPrefs.GetFloat((count - 1).ToString()) > PlayerPrefs.GetFloat(count.ToString())) {

                        float temp = PlayerPrefs.GetFloat((count - 1).ToString());
                        PlayerPrefs.SetFloat((count - 1).ToString(), PlayerPrefs.GetFloat(count.ToString()));
                        PlayerPrefs.SetFloat(count.ToString(), temp);
                    }
                    count -= 1;
                    PlayerPrefs.SetFloat("43", 500f);
                }
                else {
                    count = -1;
                }
            }


        }

        if (difficulty == 4) {

            int q = 44;
            int count = 54;


            PlayerPrefs.SetFloat(count.ToString(), Mathf.Round(timer.time * 1000) / 1000);

            while (q < count) {

                if (PlayerPrefs.HasKey((count - 1).ToString()) == true) {

                    if (PlayerPrefs.GetFloat((count - 1).ToString()) > PlayerPrefs.GetFloat(count.ToString())) {

                        float temp = PlayerPrefs.GetFloat((count - 1).ToString());
                        PlayerPrefs.SetFloat((count - 1).ToString(), PlayerPrefs.GetFloat(count.ToString()));
                        PlayerPrefs.SetFloat(count.ToString(), temp);
                    }
                    count -= 1;
                    PlayerPrefs.SetFloat("54", 500f);
                }
                else {
                    count = -1;
                }
            }
        }
    }


}

