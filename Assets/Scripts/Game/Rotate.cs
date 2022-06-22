using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector3 direction;
    [SerializeField] float speed = 10f;

    private Renderer _myRenderer;
    private Renderer MyRenderer
    {
        get
        {
            if (_myRenderer == null)
                _myRenderer = GetComponent<Renderer>();
            return _myRenderer;
        }
    }

    private void Update()
    {
        if (MyRenderer.isVisible)
            transform.Rotate(direction * speed * Time.deltaTime);
    }
}
