using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform player;
    public GameObject bow;
    public float swingSpeed = 200f;
    public float startAngleOffset = 45f;
    public float endAngleOffset = 45f;

    private SpriteRenderer swordRenderer;
    private BoxCollider2D swordCollider;
    private bool isSwinging = false;
    private float startAngle;
    private float endAngle;
    private float currentAngle;
    private float swingDirection;

    void Start()
    {
        swordRenderer = GetComponent<SpriteRenderer>();
        swordCollider = GetComponent<BoxCollider2D>();

        swordRenderer.enabled = false;
        swordCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartSwing();
        }

        if (isSwinging)
        {
            SwingSword();
        }
    }

    private void StartSwing()
    {
        Vector2 directionToCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
        float midpointAngle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;
        startAngle = midpointAngle - startAngleOffset;
        endAngle = midpointAngle + endAngleOffset;
        currentAngle = startAngle;
        transform.position = player.position;
        transform.rotation = Quaternion.Euler(0, 0, startAngle);
        swingDirection = Mathf.Sign(Mathf.DeltaAngle(startAngle, endAngle));
        swordRenderer.enabled = true;
        swordCollider.enabled = true;

        if (bow != null)
        {
            bow.SetActive(false);
        }

        isSwinging = true;
    }

    private void SwingSword()
    {
        float step = swingSpeed * Time.deltaTime * swingDirection;
        currentAngle += step;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        bool reachedEnd = swingDirection > 0 
            ? Mathf.DeltaAngle(currentAngle, endAngle) <= 0
            : Mathf.DeltaAngle(currentAngle, endAngle) >= 0;

        if (reachedEnd)
        {
            EndSwing();
        }
    }

    private void EndSwing()
    {
        isSwinging = false;
        swordRenderer.enabled = false;
        swordCollider.enabled = false;

        if (bow != null)
        {
            bow.SetActive(true);
        }
    }
}
