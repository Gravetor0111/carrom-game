using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    
    public static bool isTurnOver, playerTurn, isGameOver, isFoul, hasMadeAMove, aiTookTurn, didPlayerWin, isQueenInPocket, playerHasAcquiredQueen;
    public static bool pause;
    public static List<GameObject> playerblackPucks = new List<GameObject>();
    public static List<GameObject> aiwhitePucks = new List<GameObject>();
    public string playerWhoCanWin;

    //Current Turn Strike and Acquired Pucks
    public static List<GameObject> playerCollectedPucksInThisTurn = new List<GameObject>();

    public GameObject availablePosition, aiGo;
    public GameObject pStrikerGo, aiStrikerGo;

    public GameObject pQImg, aQImg;

    StrikerController sc;
    AI ai;
    
    public Canvas restartCanvas;
    public TextMeshProUGUI playerScore, aiScore;

    

    public Transform blackPucksParentObject, whitePucksParentObject, queenPuck, strikerPlayer, strikerAi;
    private List<GameObject> onBoardBlackPucks = new List<GameObject>();
    private List<GameObject> onBoardWhitePucks = new List<GameObject>();
    private GameObject queenGo, strikerGo;
    private Timer timer;
    private bool isDelaying = false;


    void Start()
    {
        playerWhoCanWin = "";
        didPlayerWin = false;
        isTurnOver = false;
        hasMadeAMove = false;
        isGameOver = false;
        aiStrikerGo.SetActive(false);
        playerTurn = true;
        aiTookTurn = false;
        timer = GetComponent<Timer>();
        playerblackPucks.Clear();
        aiwhitePucks.Clear();
        timer.Begin(timer.duration);
        sc = strikerPlayer.GetComponent<StrikerController>();
        ai = aiGo.GetComponent<AI>();
        restartCanvas.gameObject.SetActive(false);
        playerCollectedPucksInThisTurn.Clear();
        pQImg.SetActive(false);
        aQImg.SetActive(false);
    }

    void Update()
    {
        playerScore.text = playerblackPucks.Count.ToString();
        aiScore.text = aiwhitePucks.Count.ToString();
        if (!isGameOver && !didPlayerWin)
        {
            if (!playerTurn && !aiTookTurn)
            {
                Debug.Log("IT'S AI'S TURN");
                ai.TakeTurn();
            }
            
            CollectActiveObjects();
            if (CheckIfTurnIsOverOrNot() || (hasMadeAMove && !CheckIfObjectsInMotion()))
            {
                if (CheckIfPlayerCollectedPucks(playerTurn) && !isFoul)
                {      
                    if (isDelaying)
                        return;
                    
                    StartCoroutine(RepeatTurn());
                }
                else
                {   
                    if (isDelaying)
                        return;
                    
                    StartCoroutine(ChangeTurn());
                }
            }
        }
        else
        {
            restartCanvas.gameObject.SetActive(true);
            if (didPlayerWin)
            {
                restartCanvas.GetComponentInChildren<TextMeshProUGUI>().SetText("You Win!");
            }
            else
            {
                restartCanvas.GetComponentInChildren<TextMeshProUGUI>().SetText("You Lose, Game Over!");
            }
        }
    }

    private IEnumerator ChangeTurn()
    {
        isDelaying = true;
        isFoul = false;
        yield return new WaitForSeconds(1f);
        playerTurn = !playerTurn;
        hasMadeAMove = false;
        timer.Begin(timer.duration);
        pStrikerGo.GetComponent<CircleCollider2D>().isTrigger = true;
        aiStrikerGo.GetComponent<CircleCollider2D>().isTrigger = true;
        playerCollectedPucksInThisTurn.Clear();
        if (playerTurn)
        {
            sc.bgSpriteTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
            sc.arrowTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
            sc.circlesTransform.gameObject.SetActive(false);
            sc.strikerSlider.interactable = true;
            sc.strikerSlider.value = 0f;
            pStrikerGo.SetActive(true);
            pStrikerGo.transform.position = new Vector2(0f, -1.732406f);
            aiStrikerGo.SetActive(false);

        }
        else
        {
            sc.strikerSlider.interactable = false;
            pStrikerGo.SetActive(false);
            aiStrikerGo.SetActive(true);
            aiTookTurn = false;
        }
        isDelaying = false;
    }

    private IEnumerator RepeatTurn()
    {
        Debug.Log("Repeat Method Being Called");
        isDelaying = true;
        yield return new WaitForSeconds(1f);
        hasMadeAMove = false;
        timer.Begin(timer.duration);
        pStrikerGo.GetComponent<CircleCollider2D>().isTrigger = true;
        aiStrikerGo.GetComponent<CircleCollider2D>().isTrigger = true;
        playerCollectedPucksInThisTurn.Clear();
        if (playerTurn)
        {
            sc.strikerSlider.interactable = true;
            sc.strikerSlider.value = 0f;
            pStrikerGo.SetActive(true);
            pStrikerGo.transform.position = new Vector2(0f, -1.732406f);
            aiStrikerGo.SetActive(false);

        }
        else
        {
            sc.strikerSlider.interactable = false;
            pStrikerGo.SetActive(false);
            aiStrikerGo.SetActive(true);
            aiTookTurn = false;
        }
        isDelaying = false;
    }

    public bool CheckIfTurnIsOverOrNot()
    {
        return GameController.isTurnOver;
    }















    // CHECK IF THE OBJECTS ARE ON THE BOARD OR NOT
    private void CollectActiveObjects()
    {
        CollectActiveBlackPucks();
        CollectActiveWhitePucks();
        CollectActiveQueenPuck();
        CollectActiveStriker();
    }

    private void CollectActiveBlackPucks()
    {
        onBoardBlackPucks.Clear();

        for (int i = 0; i < blackPucksParentObject.childCount; i++)
        {
            Transform child = blackPucksParentObject.GetChild(i);
            GameObject childObject = child.gameObject;

            if (childObject.activeSelf)
            {
                onBoardBlackPucks.Add(childObject);
            }
        }
    }

    private void CollectActiveWhitePucks()
    {
        onBoardWhitePucks.Clear();

        for (int i = 0; i < whitePucksParentObject.childCount; i++)
        {
            Transform child = whitePucksParentObject.GetChild(i);
            GameObject childObject = child.gameObject;

            if (childObject.activeSelf)
            {
                onBoardWhitePucks.Add(childObject);
            }
        }
    }

    private void CollectActiveQueenPuck()
    {
        if (queenPuck.gameObject.activeSelf)
        {
            queenGo = queenPuck.gameObject;
        }
    }

    private void CollectActiveStriker()
    {
        if (playerTurn)
        {
            if (strikerPlayer.gameObject.activeSelf)
            {
                strikerGo = strikerPlayer.gameObject;
            }
        }
        else
        {
            if (strikerAi.gameObject.activeSelf)
            {
                strikerGo = strikerAi.gameObject;
            }
        }
    }

    // CHECK IF THE OBJECTS ARE IN MOTION OR NOT
    private bool CheckIfObjectsInMotion()
    {
        if (!ArePucksInMotion(onBoardBlackPucks) && !ArePucksInMotion(onBoardWhitePucks) && !IsStrikerOrQueenInMotion(queenGo) && !IsStrikerOrQueenInMotion(strikerGo))
        {
            return false;
        }
        return true;
    }

    private bool IsObjectInMotion(GameObject go)
    {
        if (go == null)
        {
            return false;
        }
        return go.GetComponent<Rigidbody2D>().velocity.magnitude > 0.01f;
    }

    private bool ArePucksInMotion(List<GameObject> list)
    {
        foreach (GameObject go in list)
        {
            if (IsObjectInMotion(go))
            {
                return true;
            }
        }
        return false;
    }
    private bool IsStrikerOrQueenInMotion(GameObject go)
    {

        if (IsObjectInMotion(go))
        {
            return true;
        }
        return false;
    }


    // CHECK IF PLAYER COLLECTED THEIR PUCKS IN THE CURRENT TURN OR NOT
    private bool CheckIfPlayerCollectedPucks(bool playerTurn)
    {
        if(playerTurn)
        {
            for (int i = 0; i < playerCollectedPucksInThisTurn.Count; i++)
            {
                if (playerCollectedPucksInThisTurn[i].CompareTag("Black") || playerCollectedPucksInThisTurn[i].CompareTag("Queen"))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            for (int i = 0; i < playerCollectedPucksInThisTurn.Count; i++)
            {
                if (playerCollectedPucksInThisTurn[i].CompareTag("White") || playerCollectedPucksInThisTurn[i].CompareTag("Queen"))
                {
                    return true;
                }
            }
            return false;
        }
        
    }

    private bool CheckIfPlayerPottedQueen(bool playerTurn)
    {
        if(playerTurn)
        {
            for (int i = 0; i < playerCollectedPucksInThisTurn.Count; i++)
            {
                if (playerCollectedPucksInThisTurn[i].CompareTag("Queen"))
                {
                    Debug.Log("QUEEN FOUND!");
                    return true;
                }
            }
            return false;
        }
        else
        {
            for (int i = 0; i < playerCollectedPucksInThisTurn.Count; i++)
            {
                if (playerCollectedPucksInThisTurn[i].CompareTag("Queen"))
                {
                    Debug.Log("QUEEN FOUND!");
                    return true;
                }
            }
            return false;
        }
        
    }

}
