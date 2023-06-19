using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float strikeForce = 50f; // Magnitude of the force applied by the AI player
    public Transform whitePucksParentObject;

    public GameObject striker;
    StrikerController sc;
    private List<GameObject> onBoardWhitePucks = new List<GameObject>();

    void Start()
    {
        sc = striker.GetComponent<StrikerController>();
    }

    void Update()
    {
        
    }

    public void TakeTurn()
    {
        striker.GetComponent<CircleCollider2D>().isTrigger = false;
        onBoardWhitePucks.Clear();
        striker.transform.position = new Vector2(0f, 1.24f);

        for (int i = 0; i < whitePucksParentObject.childCount; i++)
        {
            Transform child = whitePucksParentObject.GetChild(i);
            GameObject childObject = child.gameObject;

            if (childObject.activeSelf)
            {
                onBoardWhitePucks.Add(childObject);
            }
        }
        
        if (onBoardWhitePucks.Count > 0)
        {
            float selectedPosition = Random.Range(-1f, 1f);
            Vector2 baselinePosition = new Vector2(selectedPosition, striker.transform.position.y);
            striker.transform.position = baselinePosition;

            SelectAndStrike();
        }
        Debug.Log("AI TOOK TURN");
    }

    private void SelectAndStrike()
    {
        int randomIndex = Random.Range(0, onBoardWhitePucks.Count);
        GameObject selectedPuck = onBoardWhitePucks[randomIndex];
        Vector2 direction = selectedPuck.transform.position - striker.transform.position;
        Vector2 force = direction.normalized * strikeForce * 3f;

        
        sc.Strike(force, striker.transform);
        GameController.aiTookTurn = true;
        Debug.Log("AI STROOK STRIKER");
    }

    // private IEnumerator DelayTimer(int seconds)
    // {
    //     print("AI Timer Started");
    //     while (seconds >= 0)
    //     {
    //         seconds--;
    //         yield return new WaitForSeconds(1f);
    //     }
    // }
}
