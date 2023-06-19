using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    
    public int duration = 20;
    private int remainingDuration, mainTimerRemainingDuration;
    
    
    // [SerializeField]
    // private Image player1, ai;

    [SerializeField]
    private Image uiFillP1, uiFillAi;

    [SerializeField]
    private TextMeshProUGUI mainTimerTxt;

    void Start()
    {
        GameController.pause = false;
        BeginMainTimer(120);
    }

    void Update()
    {
        
    }

    public void Begin(int second)
    {
        GameController.isTurnOver = false;
        GameController.pause = false;
        uiFillAi.fillAmount = 1f;
        uiFillP1.fillAmount = 1f;
        remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        if (GameController.playerTurn)
        {
            while (remainingDuration >= 0)
            {
                if (!GameController.pause && !GameController.isGameOver)
                {
                    uiFillP1.fillAmount = Mathf.InverseLerp(0, duration, remainingDuration);
                    remainingDuration--;
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    uiFillP1.fillAmount = 0f;
                    break;
                }
                yield return null;
            }
            OnTimerEnd();
        }
        else
        {
            while (remainingDuration >= 0)
            {
                if (!GameController.pause && !GameController.isGameOver)
                {
                    uiFillAi.fillAmount = Mathf.InverseLerp(0, duration, remainingDuration);
                    remainingDuration--;
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    uiFillAi.fillAmount = 0f;
                    break;
                }
                yield return null;
            }
            OnTimerEnd();
        }
    }

    private void OnTimerEnd()
    {
        if (!GameController.hasMadeAMove)
        {
            GameController.isTurnOver = true;
        }
        
        print("Changing Sides" + GameController.isTurnOver);
    }

    public void BeginMainTimer(int second)
    {
        mainTimerRemainingDuration = second;
        StartCoroutine(MainTimer());
    }


    private IEnumerator MainTimer()
    {

        while (mainTimerRemainingDuration >= 0)
        {
            mainTimerTxt.text = $"{mainTimerRemainingDuration / 60:00} : {mainTimerRemainingDuration % 60:00}";
            mainTimerRemainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        GameController.isGameOver = true;
        print("GAME OVER!" + GameController.isGameOver);
    }
}
