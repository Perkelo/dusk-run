using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuGameManager : MonoBehaviour
{
    [SerializeField] private Image buttonOutline;
    private Coroutine mouseover;

    private void Start()
    {
        if (SoundManager.instance == null)
        {
            SoundManager.instance = gameObject.GetComponent<SoundManager>();
        }
        SoundManager.instance.PlaySoundFX(SoundManager.AudioFX.Intro);
    }
    public void OnStartGame()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void OnMouseEnteredButton()
    {
        Debug.Log("Mouse entered");
        buttonOutline.enabled = true;
        mouseover = StartCoroutine(SpamMouseOver());
    }

    public void OnMouseExitedButton()
    {
        Debug.Log("Mouse Exited");
        buttonOutline.enabled = false;
        StopCoroutine(mouseover);
    }

    private IEnumerator SpamMouseOver()
    {
        while (true)
        {
            SoundManager.instance.PlaySoundFX(SoundManager.AudioFX.Mouseover);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.15f));
        }
    }
}
