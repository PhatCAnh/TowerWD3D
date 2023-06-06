using UnityEngine;
using UnityEngine.Events;

public class DemoObserver : MonoBehaviour
{
    public UnityEvent theUE = new();

    // Start is called before the first frame update
    void Start()
    {
        theUE.AddListener(postStatus);
    }

    private void postStatus()
    {
        Debug.Log("Nhan duoc thong bao roi ne");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            theUE.Invoke();
        }
    }
}
