using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefab genérico de ingrediente")]
    public GameObject ingredientePrefab;

    [Header("Sprites (mismo orden que iconoId en el JSON)")]
    public List<Sprite> sprites;

    [Header("Puntos donde aparecerán los ingredientes")]
    public List<Transform> puntosDeSpawn;

    void Start()
    {
        StartCoroutine(EsperarDatosYSpawnear());
    }

    IEnumerator EsperarDatosYSpawnear()
    {
        float timeout = 5f;
        float elapsed = 0f;
        while (GameManager.Instance.datos == null)
        {
            elapsed += Time.deltaTime;
            if (elapsed > timeout)
            {
                Debug.LogError("[Spawner] Timeout: datos del JSON no cargaron.");
                yield break;
            }
            yield return null;
        }

        SpawnearIngredientes();
    }

    void SpawnearIngredientes()
    {
        var ingredientes = GameManager.Instance.datos.ingredientes;
        int cantidad = Mathf.Min(ingredientes.Count, puntosDeSpawn.Count);

        List<Transform> puntosAleatorios = new List<Transform>(puntosDeSpawn);
        for (int i = 0; i < puntosAleatorios.Count; i++)
        {
            int j = Random.Range(i, puntosAleatorios.Count);
            (puntosAleatorios[i], puntosAleatorios[j]) =
            (puntosAleatorios[j], puntosAleatorios[i]);
        }

        for (int i = 0; i < cantidad; i++)
        {
            IngredienteData data = ingredientes[i];
            Sprite sprite = ObtenerSprite(data.iconoId, i);

            GameObject obj = Instantiate(
                ingredientePrefab,
                puntosAleatorios[i].position,
                Quaternion.identity
            );

            obj.GetComponent<ItemRecolectable>()
               .Inicializar(data.nombre, data.valor, sprite);
        }

        Debug.Log($"[Spawner] Se generaron {cantidad} ingredientes.");
    }

   
    Sprite ObtenerSprite(string iconoId, int indice)
    {
        Sprite s = Resources.Load<Sprite>("Sprites/" + iconoId);
        if (s != null) return s;

        if (indice >= 0 && indice < sprites.Count && sprites[indice] != null)
            return sprites[indice];

        Debug.LogWarning($"[Spawner] Sprite no encontrado para: {iconoId}");
        return null;
    }
}