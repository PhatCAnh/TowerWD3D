using UnityEngine;

public class Selection : MonoBehaviour
{
    Vector2 mousePos;
    Ray ray;
    RaycastHit hit;
    [SerializeField] private LayerMask layer;

    private Transform highlight;
    private MeshRenderer theMR_highlight => highlight.GetComponent<MeshRenderer>();
    public Node selection;

    [SerializeField] private Material highlightMaterial;
    private Material originalColor;
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 500f, Color.white);
        //hit = Physics.Raycast(ray, out hit);
        Touched();
        Clicked();
    }

    private void Touched()
    {
        if (highlight != null)
        {
            theMR_highlight.material = originalColor;

            highlight = null;
        }
        if (!Physics.Raycast(ray, out hit, 500f, layer)) return;
        highlight = hit.transform;
        if (highlight != selection)
        {
            if (theMR_highlight.material != highlightMaterial)
            {
                originalColor = theMR_highlight.material;
                theMR_highlight.material = highlightMaterial;

            }
        }
        else
        {
            highlight = null;
        }
    }

    private void Clicked()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out hit, 500f, layer))
            {
                var node = hit.transform.GetComponentInParent<Node>();
                if (selection != null && node != selection)
                {
                    selection.Unselected();
                    selection = null;
                }

                if (selection != node && !node.isHaveTower)
                {
                    selection = node;
                    if (hit.transform.gameObject.layer == 29)
                    {
                        selection.Selected();
                    }
                    else
                    {
                        selection = null;
                    }
                }
            }
            else if (selection != null)
            {
                selection.Unselected();
                selection = null;
            }
        }
    }
}
