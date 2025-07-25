using UnityEngine;

public class PingEffect : MonoBehaviour
{
    [SerializeField] private float lifetime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlaySFX("Missing");
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
