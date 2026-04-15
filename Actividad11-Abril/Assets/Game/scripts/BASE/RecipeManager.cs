using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }

    // Ingredientes entregados para la receta activa
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

    // Intenta agregar un ingrediente a la receta activa
    // Retorna: "ok", "incorrecto", "exceso", "completada", "sin_stock"
    public string EntregarIngrediente(string nombre)
    {
        RecetaData receta = ObtenerRecetaActual();
        if (receta == null) return "sin_receta";

        // Verificar si el ingrediente pertenece a la receta
        ObjetivoReceta objetivo = receta.objetivos.Find(o => o.ingrediente == nombre);
        if (objetivo == null) return "incorrecto";

        // Verificar stock en inventario
        if (GameManager.Instance.CantidadIngrediente(nombre) <= 0) return "sin_stock";

        // Verificar que no se entregue de más
        int yaEntregado = entregados.ContainsKey(nombre) ? entregados[nombre] : 0;
        if (yaEntregado >= objetivo.cantidad) return "exceso";

        // Consumir del inventario y registrar entrega
        GameManager.Instance.ConsumirIngrediente(nombre, 1);
        entregados[nombre] = yaEntregado + 1;

        // Verificar si la receta está completa
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