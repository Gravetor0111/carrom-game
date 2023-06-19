using UnityEngine;

public class PocketController : MonoBehaviour
{
    public GameObject gcObj;
    GameController gc;

    void Start()
    {
        gcObj = GameObject.Find("GameController");
        gc = gcObj.GetComponent<GameController>();
        Debug.Log(gcObj.name);
    }

    void Update()
    {
        if (GameController.playerblackPucks.Count == 9)
        {
            GameController.didPlayerWin = true;
        }
        else if (GameController.aiwhitePucks.Count == 9)
        {
            GameController.didPlayerWin = false;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Black") || collision.CompareTag("White"))
        {
            string puckTag = collision.gameObject.tag;
            if (puckTag == "Black")
            {
                GameController.playerblackPucks.Add(collision.gameObject);
                Debug.Log("Black Total: " + GameController.playerblackPucks.Count);
                if (GameController.playerTurn)
                    GameController.playerCollectedPucksInThisTurn.Add(collision.gameObject);
            }
            else if (puckTag == "White")
            {
                GameController.aiwhitePucks.Add(collision.gameObject);
                Debug.Log("White Total: " + GameController.aiwhitePucks.Count);
                if (!GameController.playerTurn)
                    GameController.playerCollectedPucksInThisTurn.Add(collision.gameObject);
            }
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Queen"))
        {
            collision.gameObject.SetActive(false);
            GameController.isQueenInPocket = true;
            GameController.playerCollectedPucksInThisTurn.Add(collision.gameObject);
            if (GameController.playerTurn)
            {
                gc.pQImg.SetActive(true);
            }
            else
            {
                gc.aQImg.SetActive(true);
            }

        }

        if (collision.CompareTag("Striker"))
        {
            collision.gameObject.SetActive(false);
            GameController.isFoul = true;
        }
    }
}