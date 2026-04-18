using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }

   
    private Dictionary<string, int> entregados = new Dictionary<string, int>();

    void Awake()
    {
        Instance = this;
    }

    public RecetaData ObtenerRecetaActual()
    {
        var recetas = GameManager.Instance.datos?.recetas;
        if (recetas == null) return null;
        int idx = GameManager.Instance.recetaActualIndex;
        return (idx < recetas.Count) ? recetas[idx] : null;
    }

   
    public string EntregarIngrediente(string nombre)
    {
        RecetaData receta = ObtenerRecetaActual();
        if (receta == null) return "sin_receta";

        
        ObjetivoReceta objetivo = receta.objetivos.Find(o => o.ingrediente == nombre);
        if (objetivo == null) return "incorrecto";

       
        if (GameManager.Instance.CantidadIngrediente(nombre) <= 0) return "sin_stock";

        
        int yaEntregado = entregados.ContainsKey(nombre) ? entregados[nombre] : 0;
        if (yaEntregado >= objetivo.cantidad) return "exceso";

        
        GameManager.Instance.ConsumirIngrediente(nombre, 1);
        entregados[nombre] = yaEntregado + 1;

        
        if (RecetaCompleta(receta)) return "completada";

        return "ok";
    }

    bool RecetaCompleta(RecetaData receta)
    {
        foreach (var obj in receta.objetivos)
        {
            int entregado = entregados.ContainsKey(obj.ingrediente) ? entregados[obj.ingrediente] : 0;
            if (entregado < obj.cantidad) return false;
        }
        return true;
    }

    public void AvanzarReceta()
    {
        entregados.Clear();
        GameManager.Instance.recetaActualIndex++;
        GameManager.Instance.recetasCompletadas++;
    }

    public bool HayMasRecetas()
    {
        var recetas = GameManager.Instance.datos?.recetas;
        return recetas != null && GameManager.Instance.recetaActualIndex < recetas.Count;
    }

    public Dictionary<string, int> GetEntregados() => entregados;
}