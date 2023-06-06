using UnityEngine;

public class Selection : MonoBehaviour
{
    Vector2 mousePos;
    Ray ray;
    RaycastHit2D hit;

    private Transform highlight;
    private SpriteRenderer theSR_highlight => highlight.GetComponent<SpriteRenderer>();
    private Transform selection;
    private SpriteRenderer theSR_selection => selection.GetComponent<SpriteRenderer>();
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color selectionColor;
    private Color originalColor;
    void Update()
    {
        mousePos = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(mousePos);
        Debug.DrawRay(ray.origin, ray.direction * 500f, Color.white);
        hit = Physics2D.Raycast(ray.origin, Vector2.zero);
        Touched();
        Clicked();
    }

    private void Touched()
    {
        if (highlight != null)
        {
            theSR_highlight.color = originalColor;
            highlight = null;
        }

        if (hit && hit.transform.gameObject.layer == 29)
        {
            highlight = hit.transform;
            if (highlight != selection)
            {   
                if (theSR_highlight.color != highlightColor)
                {
                    originalColor = theSR_highlight.color;
                    theSR_highlight.color = highlightColor;
                }
            }
            else
            {
                highlight = null;
            }
        }
    }

    private void Clicked()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (selection != null)
            {
                theSR_selection.color = originalColor;
                selection = null;
            }
            selection = hit.transform;
            if (hit && hit.transform.gameObject.layer == 29)
            {
                theSR_selection.color = selectionColor;
            }
            else
            {
                selection = null;
            }
        }
    }
}
