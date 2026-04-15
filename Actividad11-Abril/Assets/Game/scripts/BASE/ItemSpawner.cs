using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefab genérico de ingrediente")]
    public GameObject ingredientePrefab;

    [Header("Sprites para cada iconoId (mismo orden que el JSON)")]
    public List<Sprite> sprites; // Asigna en Inspector: hongo, raiz, polvo, escama

    [Header("Puntos de spawn en el mapa")]
    public List<Transform> puntosDeSpawn;

    void Start()
    {
        if (GameManager.Instance.datos == null)
        {
            Debug.LogWarning("[Spawner] Datos aún no cargados.");
            return;
        }
        SpawnearIngredientes();
    }

    void SpawnearIngredientes()
    {
        var ingredientes = GameManager.Instance.datos.ingredientes;
        int cantidad = Mathf.Min(ingredientes.Count, puntosDeSpawn.Count);

        // Mezclar puntos de spawn aleatoriamente
        List<Transform> puntosAleatorios = new List<Transform>(puntosDeSpawn);
        for (int i = 0; i < puntosAleatorios.Count; i++)
        {
            int j = Random.Range(i, puntosAleatorios.Count);
            (puntosAleatorios[i], puntosAleatorios[j]) = (puntosAleatorios[j], puntosAleatorios[i]);
        }

        for (int i = 0; i < cantidad; i++)
        {
            IngredienteData data = ingredientes[i];
            Sprite sprite = ObtenerSprite(data.iconoId);

            GameObject obj = Instantiate(ingredientePrefab, puntosAleatorios[i].position, Quaternion.identity);
            obj.GetComponent<ItemRecolectable>().Inicializar(data.nombre, data.valor, sprite);
        }
    }

    Sprite ObtenerSprite(string iconoId)
    {
        // Intentar cargar desde Resources primero
        Sprite s = Resources.Load<Sprite>("Sprites/" + iconoId);
        if (s != null) return s;

        // Fallback: buscar en la lista asignada en el Inspector por índice
        var ingredientes = GameManager.Instance.datos.ingredientes;
        int idx = ingredientes.FindIndex(x => x.iconoId == iconoId);
        if (idx >= 0 && idx < sprites.Count) return sprites[idx];

        Debug.LogWarning($"[Spawner] Sprite no encontrado para: {iconoId}");
        return null;
    }
}