using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Ping : MonoBehaviour
{
    public GameObject selected;
    public RectTransform iconRt;
    RectTransform rt;
    Vector3 originalScaleIconRt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rt = GetComponent<RectTransform>();
        originalScaleIconRt = iconRt.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, Camera.main))
        {
            selected.SetActive(true);
            iconRt.localScale = originalScaleIconRt + new Vector3(0.25f, 0.25f, 0.25f);
        }
        else
        {
            selected.SetActive(false);
            iconRt.localScale = originalScaleIconRt;
        }
    }
}
