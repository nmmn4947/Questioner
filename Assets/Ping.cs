using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Ping : MonoBehaviour
{
    public GameObject selected;
    public RectTransform iconRt;
    public GameObject pingFXPrefab;
    RectTransform rt;
    RectTransform rtParent;
    Vector3 originalScaleIconRt;
    public bool isSelected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rt = GetComponent<RectTransform>();
        rtParent = GetComponentInParent<RectTransform>();
        originalScaleIconRt = iconRt.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, Camera.main))
        {
            selected.SetActive(true);
            PingWheel.instance.assignSelectedPing(this);
            isSelected = true;
            iconRt.localScale = originalScaleIconRt + new Vector3(0.25f, 0.25f, 0.25f);
        }
        else
        {
            selected.SetActive(false);
            isSelected = false;
            iconRt.localScale = originalScaleIconRt;
        }
    }

    public void InstantiatePing(RectTransform rta, GameObject parent)
    {
        Instantiate(pingFXPrefab, rta.position, Quaternion.identity, parent.transform);
    }
}
