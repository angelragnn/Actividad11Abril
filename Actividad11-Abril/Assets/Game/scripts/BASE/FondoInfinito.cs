using UnityEngine;

public class FondoInfinito : MonoBehaviour
{
    [Header("Referencia")]
    public Transform camara;

    [Header("Parallax (0 = sigue cámara exacto, 1 = fijo en mundo)")]
    public float velocidadParallaxX = 0f;
    public float velocidadParallaxY = 0f;

    [Header("Tamaño del fondo (ajusta hasta cubrir pantalla)")]
    public float escala = 1f;

    private Material material;
    private Vector3 posicionAnteriorCamara;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;

        if (camara == null)
            camara = Camera.main.transform;

        posicionAnteriorCamara = camara.position;

        // Escalar el fondo para que cubra toda la pantalla
        transform.localScale = new Vector3(escala, escala, 1f);
    }

    void LateUpdate()
    {
        Vector3 delta = camara.position - posicionAnteriorCamara;

        // Mover el fondo siguiendo la cámara en X e Y
        float moveX = delta.x * (1 - velocidadParallaxX);
        float moveY = delta.y * (1 - velocidadParallaxY);
        transform.position += new Vector3(moveX, moveY, 0);

        // Mover el tiling para efecto infinito en X
        Vector2 offset = material.mainTextureOffset;
        offset.x += delta.x * velocidadParallaxX * 0.1f;
        material.mainTextureOffset = offset;

        posicionAnteriorCamara = camara.position;
    }
}