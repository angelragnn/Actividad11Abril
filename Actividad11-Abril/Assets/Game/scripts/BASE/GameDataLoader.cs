using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GameDataLoader : MonoBehaviour
{
    void Start()
    {
        // Solo carga si el GameManager aún no tiene datos
        if (GameManager.Instance.datos == null)
            StartCoroutine(CargarDatos());
    }

    IEnumerator CargarDatos()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "datos.json");

        // UnityWebRequest funciona en todas las plataformas (incluye Android)
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            GameData data = JsonUtility.FromJson<GameData>(request.downloadHandler.text);
            GameManager.Instance.SetDatos(data);
            Debug.Log($"[DataLoader] Cargados {data.ingredientes.Count} ingredientes y {data.recetas.Count} recetas.");
        }
        else
        {
            Debug.LogError("[DataLoader] Error al cargar JSON: " + request.error);
        }
    }
}