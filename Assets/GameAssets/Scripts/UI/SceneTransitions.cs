using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            LoadTheNextDay();
        }
    }

    public void LoadTheNextDay()
    {
        StartCoroutine(LoadNextScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadNextScene(int sceneIndex)
    {
        GetComponent<Animator>().SetTrigger("Out");
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(sceneIndex);
    }
}
