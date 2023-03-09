/*
	Copyright (C) 2020 Anarres

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/> 
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public float transitionTime = 1f;
    public Animator transition;

    public bool isMainMenu = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMainMenu)
            {
                FadeAnExit();
            }
            else
            {
                FadeAndLoadScene("Menu");
            }
            
        }
    }

    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    public void FadeAnExit()
    {
        StartCoroutine(ExitGame());
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        Debug.Log("Crossfading");
        yield return new WaitForSeconds(transitionTime);

        Debug.Log("Load level!" + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator ExitGame()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        Application.Quit();
    }

}
