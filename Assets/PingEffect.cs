using UnityEngine;

public class PingEffect : MonoBehaviour
{
    [SerializeField] private float lifetime;
    [SerializeField] private string pingSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlaySFX(pingSound);
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
