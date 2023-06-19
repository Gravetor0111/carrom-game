using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrikerController : MonoBehaviour {
    
    //private float maxSpeed = 9f;

    [SerializeField]
    public Slider strikerSlider;
    [SerializeField]
    public Transform bgSpriteTransform;
    [SerializeField]
    public Transform arrowTransform;
    [SerializeField]
    public Transform circlesTransform;

    public float dotsDistanceFromCircle = 10f;
    public float forceScale = 1f;

    private CircleCollider2D strikerCollider;
    private float minScale = 0.10f;
    private float maxScale = 1.2f;
    private float scaleFactor = 0.9f;

    private bool isOverlapping = false;
    private bool isDragging = false;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    


    private void Start()
    {
        strikerCollider = GetComponent<CircleCollider2D>();
        strikerCollider.isTrigger = true;
        strikerSlider.onValueChanged.AddListener(StrikerPosition);
        initialScale = bgSpriteTransform.localScale;
        initialRotation = arrowTransform.rotation;
        circlesTransform.gameObject.SetActive(false);
    }

    private void Update()
    {

    }

    private void OnMouseDown()
    {
        if (GameController.playerTurn && !GameController.hasMadeAMove && !GameController.isGameOver)
        {
            isDragging = true;
            circlesTransform.gameObject.SetActive(true);
        }
        
    }

    private void OnMouseUp()
    {
        if (GameController.playerTurn && !GameController.hasMadeAMove && !GameController.isGameOver)
        {
            isDragging = false;
            float scale = 0f;

            if (bgSpriteTransform.localScale.x >= maxScale)
            {
                scale = 1.2f;
            }
            else
            {
                scale = bgSpriteTransform.localScale.x;
            }
            
            float forceMagnitude = scale * forceScale * 20f;

            Vector3 direction = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 force = direction.normalized * forceMagnitude;

            if (!isDragging && scale > 0.17f)
            {
                Strike(force, transform);
            }

            bgSpriteTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
            arrowTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
            circlesTransform.gameObject.SetActive(false);
            
        }
        
    }

    private void OnMouseDrag()
    {
        if (GameController.playerTurn && !GameController.hasMadeAMove && !GameController.isGameOver)
        {
            if (isDragging)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float distance = Vector2.Distance(mousePosition, transform.position);

                float scale = Mathf.Clamp(initialScale.x + distance * scaleFactor, minScale, maxScale);
                bgSpriteTransform.localScale = new Vector3(scale, scale, 1f);
                arrowTransform.localScale = new Vector3(scale, scale, 1f);

                Vector3 direction = transform.position - mousePosition;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrowTransform.rotation = Quaternion.Euler(0f, 0f, angle);

                // Align dots sprite with the edge of the circle
                float circleRadius = bgSpriteTransform.localScale.x * 0.5f;
                Vector3 dotsPosition = bgSpriteTransform.position + (direction.normalized * (circleRadius + dotsDistanceFromCircle));
                circlesTransform.position = dotsPosition;

                // Rotate dots sprite relative to the striker
                circlesTransform.rotation = Quaternion.Euler(0f, 0f, arrowTransform.rotation.eulerAngles.z);

                if (circlesTransform.localScale.x >= maxScale)
                {
                    circlesTransform.localScale = new Vector3(maxScale, maxScale, 1f);
                }
                else
                {
                    circlesTransform.localScale = arrowTransform.localScale * 0.75f;
                }
            }
        }
    }


    public void StrikerPosition(float Value)
    {
        transform.position = new Vector2(Value, -1.732406f);
    }

    public void Strike(Vector2 dirNFor, Transform striker)
    {
        if (!isOverlapping)
        {
            strikerSlider.interactable = false;
            strikerCollider.isTrigger = false;
            GameController.pause = true;
            GameController.hasMadeAMove = true;
            striker.GetComponent<Rigidbody2D>().AddForce(dirNFor, ForceMode2D.Impulse);
        }
    }

}