// Assets/Game/scripts/BASE/ItemRecolectable.cs
using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    // Datos del ingrediente (asignados por ItemSpawner)
    [HideInInspector] public string nombreIngrediente;
    [HideInInspector] public int valor;

    /// <summary>
    /// Llamado por ItemSpawner para configurar el prefab dinámicamente.
    /// Asigna nombre, valor y sprite según los datos del JSON.
    /// </summary>
    public void Inicializar(string nombre, int val, Sprite sprite)
    {
        nombreIngrediente = nombre;
        valor = val;
        GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.name = nombre; // útil para depuración
    }

    /// <summary>
    /// Se ejecuta cuando el jugador (tag "Player") entra en el trigger.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1. Guardar en el GameManager (actualiza el inventario)
        GameManager.Instance.AgregarIngrediente(nombreIngrediente);

        // 2. Mostrar mensaje en UI
        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null)
            ui.MostrarMensaje($"¡Recogiste: {nombreIngrediente}!");

        // 3. Destruir el objeto del mundo
        Destroy(gameObject);
    }
}