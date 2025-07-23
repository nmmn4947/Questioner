using System.Collections;
using UnityEngine;

public class Ping : MonoBehaviour
{
    static public Ping instance;
    RectTransform rt;
    CanvasGroup cg;
    [SerializeField] private float fadeInTime;
    private float fadeInKeep = 0.0f;
    private bool isActivate = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rt = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            fadeInKeep += Time.deltaTime;
            Debug.Log(fadeInKeep / fadeInTime);
            cg.alpha = Mathf.Lerp(0, 1, fadeInKeep / fadeInTime);
        }
        else
        {

        }
    }

    public void activatePing()
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.GetComponent<RectTransform>(), // The Canvas's RectTransform
            Input.mousePosition,                      // The screen point (mouse position)
            parentCanvas.worldCamera,                 // The camera assigned to the Canvas's 'Render Camera'
            out localPoint                            // The resulting local point
        );

        rt.anchoredPosition = localPoint;
        isActivate = true;
    }

    public void deActivatePing()
    {
        isActivate = false;
        cg.alpha = 0.0f;
        fadeInKeep = 0.0f;
    }
    
}
