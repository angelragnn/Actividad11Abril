using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

   
    public GameData datos { get; private set; }

    
    public Dictionary<string, int> inventario = new Dictionary<string, int>();

    
    public int recetaActualIndex = 0;

   
    public int recetasCompletadas = 0;

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetDatos(GameData d) => datos = d;

    public void AgregarIngrediente(string nombre)
    {
        if (inventario.ContainsKey(nombre))
            inventario[nombre]++;
        else
            inventario[nombre] = 1;

        Debug.Log($"[Inventario] {nombre} x{inventario[nombre]}");
    }

    public int CantidadIngrediente(string nombre)
    {
        return inventario.ContainsKey(nombre) ? inventario[nombre] : 0;
    }

    public bool ConsumirIngrediente(string nombre, int cantidad)
    {
        if (CantidadIngrediente(nombre) >= cantidad)
        {
            inventario[nombre] -= cantidad;
            return true;
        }
        return false;
    }

    public int TotalIngredientes()
    {
        int total = 0;
        foreach (var kvp in inventario) total += kvp.Value;
        return total;
    }

    public void ResetearJuego()
    {
        inventario.Clear();
        recetaActualIndex = 0;
        recetasCompletadas = 0;
    }
}