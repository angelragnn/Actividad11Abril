using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    [HideInInspector] public string nombreIngrediente;
    [HideInInspector] public int valor;

    // Llamado por ItemSpawner para configurar el prefab dinámicamente
    public void Inicializar(string nombre, int val, Sprite sprite)
    {
        nombreIngrediente = nombre;
        valor = val;
        GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.name = nombre;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AgregarIngrediente(nombreIngrediente);

            // Notificar UI si existe en la escena
            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null) ui.MostrarMensaje($"¡Recogiste: {nombreIngrediente}!");

            Destroy(gameObject);
        }
    }
}