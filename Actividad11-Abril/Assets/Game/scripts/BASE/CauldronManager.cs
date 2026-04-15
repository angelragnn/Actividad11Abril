using UnityEngine;
using UnityEngine.SceneManagement;

public class CauldronManager : MonoBehaviour
{
    private UIManager ui;

    void Start()
    {
        ui = FindObjectOfType<UIManager>();
        MostrarRecetaActual();
    }

    void MostrarRecetaActual()
    {
        var receta = RecipeManager.Instance.ObtenerRecetaActual();
        if (receta != null && ui != null)
            ui.ActualizarReceta(receta);
    }

    // Llamar desde un botón de UI o zona de entrega pasando el nombre del ingrediente
    public void IntentarEntregar(string nombreIngrediente)
    {
        string resultado = RecipeManager.Instance.EntregarIngrediente(nombreIngrediente);

        switch (resultado)
        {
            case "ok":
                ui?.MostrarMensaje($"✔ {nombreIngrediente} agregado.");
                ui?.ActualizarProgreso(RecipeManager.Instance.GetEntregados(),
                                       RecipeManager.Instance.ObtenerRecetaActual());
                break;

            case "completada":
                ui?.MostrarMensaje($"🎉 ¡Receta completada: {RecipeManager.Instance.ObtenerRecetaActual().nombre}!");
                RecipeManager.Instance.AvanzarReceta();
                Invoke(nameof(SiguienteRecetaOVictoria), 1.5f);
                break;

            case "incorrecto":
                ui?.MostrarMensaje("❌ Ingrediente incorrecto para esta receta.");
                break;

            case "exceso":
                ui?.MostrarMensaje("⚠ Ya entregaste suficiente de ese ingrediente.");
                break;

            case "sin_stock":
                ui?.MostrarMensaje("⚠ No tienes ese ingrediente en el inventario.");
                break;
        }
    }

    void SiguienteRecetaOVictoria()
    {
        if (RecipeManager.Instance.HayMasRecetas())
            MostrarRecetaActual();
        else
            ui?.MostrarVictoria();
    }
}