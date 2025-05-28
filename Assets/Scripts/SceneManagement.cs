using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { get; private set; }

    public event EventHandler OnGameStart;

    [SerializeField] private GameObject startGameCanvas;
    [SerializeField] private GameObject meltBG;
    [SerializeField] private float startPosY, endPosY;
    [SerializeField] private float meltAnimTime;

    private void Awake() 
    {
        Instance = this;
        InitializeUI();
    }

    private void InitializeUI()
    {
        startGameCanvas.SetActive(true);
        RectTransform meltBGRect = meltBG.GetComponent<RectTransform>();
        meltBGRect.localPosition = new Vector3(0, startPosY, meltBGRect.position.z);
        startGameCanvas.transform.GetChild(0).GetComponent<Button>().interactable = false;
        startGameCanvas.transform.GetChild(0).localScale = new Vector3(0, 0, 1);
        LeanTween.scale(startGameCanvas.transform.GetChild(0).gameObject, new Vector3(1, 1, 1), 0.4f).setEaseInCubic().setOnComplete(() => {
            startGameCanvas.transform.GetChild(0).GetComponent<Button>().interactable = true;
        });
    }

    public void MeltTransition(System.Action action)
    {
        RectTransform meltBGRect = meltBG.GetComponent<RectTransform>();
        Vector3 pos = meltBGRect.localPosition;
        LeanTween.moveLocal(meltBG, new Vector3(0, endPosY, pos.z), meltAnimTime).setEaseInOutSine().setOnComplete(action);
    }

    public void MeltTransition()
    {
        RectTransform meltBGRect = meltBG.GetComponent<RectTransform>();
        Vector3 pos = meltBGRect.localPosition;
        LeanTween.moveLocal(meltBG, new Vector3(0, endPosY, pos.z), meltAnimTime).setEaseInOutSine();
    }

    public void RestartMeltTransition()
    {
        RectTransform meltBGRect = meltBG.GetComponent<RectTransform>();
        Vector3 pos = meltBGRect.localPosition;
        LeanTween.moveLocal(meltBG, new Vector3(0, startPosY, pos.z), meltAnimTime).setEaseInOutSine().setOnComplete(() => {
            ResetAnimation();
        });
    }

    private void ResetAnimation()
    {
        StartCoroutine(RemoveAllBlocks());
    }

    private IEnumerator RemoveAllBlocks()
    {
        List<Block> blocks = new List<Block>(FindObjectsOfType<Block>());
        foreach (Block block in blocks)
        {
            block.ScaleOutAnimation(0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("GameScene");
    }

    public void RestartGame()
    {
        GameOverUI.Instance.ClosePopup();
    }

    public void StartGame()
    {
        startGameCanvas.SetActive(false);
        StartCoroutine(DelayedStart(0.5f));
    }

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }
}
