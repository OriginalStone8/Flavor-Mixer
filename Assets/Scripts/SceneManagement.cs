using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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

    [SerializeField] private List<Button> shopBtns;
    [SerializeField] private GameObject tilesBG;

    private void Awake()
    {
        Instance = this;
        InitializeUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void InitializeUI()
    {
        startGameCanvas.SetActive(true);
        RectTransform meltBGRect = meltBG.GetComponent<RectTransform>();
        meltBGRect.localPosition = new Vector3(0, startPosY, meltBGRect.position.z);
        startGameCanvas.transform.GetChild(0).GetComponent<Button>().interactable = false;
        startGameCanvas.transform.GetChild(0).localScale = new Vector3(0, 0, 1);
        LeanTween.scale(startGameCanvas.transform.GetChild(0).gameObject, new Vector3(1, 1, 1), 0.4f).setEaseInCubic().setOnComplete(() =>
        {
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

    public void RestartMeltTransition(bool gameOver)
    {
        RectTransform meltBGRect = meltBG.GetComponent<RectTransform>();
        Vector3 pos = meltBGRect.localPosition;
        LeanTween.moveLocal(meltBG, new Vector3(0, startPosY, pos.z), meltAnimTime).setEaseInOutSine().setOnComplete(() =>
        {
            if (gameOver)
            {
                ResetAnimation();
            }
            else
            {
                //continue
                ContinueGame.Instance.DestroyForContinue();
            }
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
        OrdersManager.Instance.CloseOrderPopup();
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("GameScene");
    }

    public void RestartGame()
    {
        GameOverUI.Instance.ClosePopup(true);
    }

    public void ContinuePlaying()
    {
        GameOverUI.Instance.ClosePopup(false);
    }

    public void StartGame()
    {
        startGameCanvas.SetActive(false);
        StartCoroutine(DelayedStart(0.5f));
    }

    public void OpenPopup(GameObject popup)
    {
        popup.SetActive(true);
        PopupAnim(popup, true, 0.2f);
        if (popup.CompareTag("Shop"))
        {
            foreach (Button btn in shopBtns)
            {
                btn.interactable = false;
            }
            tilesBG.SetActive(false);
            ShopItemManager.Instance.UpdateItems();
            ShopItemManager.Instance.UpdateItemsButton();
        }
    }

    public void ClosePopup(GameObject popup)
    {
        PopupAnim(popup, false, 0.2f);
        if (popup.CompareTag("Shop"))
        {
            foreach (Button btn in shopBtns)
            {
                btn.interactable = true;
            }
            tilesBG.SetActive(true);
        }
    }

    private void PopupAnim(GameObject popup, bool on, float time)
    {
        if (on)
        {
            popup.transform.localScale = new Vector3(0, 0, 0);
            LeanTween.scale(popup, new Vector3(1, 1, 1), time).setEaseOutBack();
        }
        else
        {
            LeanTween.scale(popup, new Vector3(0, 0, 0), time).setEaseInBack().setOnComplete(() =>
            {
                popup.SetActive(false);
            });
        }
    }

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }
    
    public IEnumerator DelayedContinue(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManagerScript.Instance.SetGameState(GameManagerScript.GameState.canSwipe);
    }
}
