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
        while (GameManager.Instance == null || GameManager.Instance.datos == null)
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

        
        List<IngredienteData> listaSpawn = new List<IngredienteData>();

        foreach (var receta in GameManager.Instance.datos.recetas)
        {
            foreach (var objetivo in receta.objetivos)
            {
                
                IngredienteData data = ingredientes.Find(i => i.nombre == objetivo.ingrediente);
                if (data == null) continue;

                
                for (int c = 0; c < objetivo.cantidad; c++)
                    listaSpawn.Add(data);
            }
        }

        
        while (listaSpawn.Count < puntosDeSpawn.Count)
        {
            listaSpawn.Add(ingredientes[Random.Range(0, ingredientes.Count)]);
        }

        
        List<Transform> puntosAleatorios = new List<Transform>(puntosDeSpawn);
        for (int i = 0; i < puntosAleatorios.Count; i++)
        {
            int j = Random.Range(i, puntosAleatorios.Count);
            (puntosAleatorios[i], puntosAleatorios[j]) = (puntosAleatorios[j], puntosAleatorios[i]);
        }

        int cantidad = Mathf.Min(listaSpawn.Count, puntosAleatorios.Count);

        for (int i = 0; i < cantidad; i++)
        {
            IngredienteData data = listaSpawn[i];
            int indice = ingredientes.IndexOf(data);
            Sprite sprite = ObtenerSprite(data.iconoId, indice);

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