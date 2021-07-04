using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuGameManager : MonoBehaviour {

    [SerializeField] private Image storyButtonOutline;
    [SerializeField] private Image brawlButtonOutline;
    [SerializeField] private Image quitButtonOutline;
    [SerializeField] private Image free;
    [SerializeField] private Image andKnuckles;
    private Coroutine mouseover;

    private void Start()
    {
        if (AudioManager.instance == null)
        {
            AudioManager.instance = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }
        AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.Intro);
        StartCoroutine(AndKnuckles());
    }

    public void OnStartStoryMode() {
        SceneManager.LoadScene("LevelScene", LoadSceneMode.Single);
    }

    public void OnStartInfiniteMode()
    {
        SceneManager.LoadScene("LevelScene", LoadSceneMode.Single);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
    public void OnMouseEnteredStoryModeButton() {
        //Debug.Log("Mouse entered");
        storyButtonOutline.enabled = true;
        mouseover = StartCoroutine(SpamMouseOver());
    }

    public void OnMouseExitedStoryModeButton() {
        //Debug.Log("Mouse Exited");
        storyButtonOutline.enabled = false;
        StopCoroutine(mouseover);
    }

    public void OnMouseEnteredBrawlButton() {
        //Debug.Log("Mouse entered");
        brawlButtonOutline.enabled = true;
        mouseover = StartCoroutine(SpamMouseOver());
    }

    public void OnMouseExitedBrawlButton() {
        //Debug.Log("Mouse Exited");
        brawlButtonOutline.enabled = false;
        StopCoroutine(mouseover);
    }

    public void OnMouseEnteredQuitButton()
    {
        Debug.Log("Mouse entered");
        quitButtonOutline.enabled = true;
        mouseover = StartCoroutine(SpamMouseOver());
    }

    public void OnMouseExitedQuitButton()
    {
        Debug.Log("Mouse Exited");
        quitButtonOutline.enabled = false;
        StopCoroutine(mouseover);
    }

    private IEnumerator SpamMouseOver()
    {
        while (true)
        {
            AudioManager.instance.PlaySoundFX(AudioManager.AudioFX.Mouseover);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.15f));
        }
    }

    private IEnumerator AnimateFreeText()
    {
        while (true)
        {
            free.rectTransform.sizeDelta = new Vector2(free.rectTransform.sizeDelta.x + Mathf.Sin(Time.time * 1.5f) * 20, free.rectTransform.sizeDelta.y + Mathf.Sin(Time.time * 1.5f) * 20);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator AndKnuckles()
    {
        yield return new WaitForSeconds(5.5f);
        andKnuckles.enabled = true;
        yield return new WaitForSeconds(2f);
        free.enabled = true;
        StartCoroutine(AnimateFreeText());
    }
}
