using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD General")]
    public TextMeshProUGUI textoMensaje;
    public TextMeshProUGUI textoInventario;

    [Header("Laboratorio")]
    public TextMeshProUGUI textoRecetaActual;
    public TextMeshProUGUI textoProgreso;
    public GameObject panelVictoria;

    [Header("Escena Recolección")]
    public TextMeshProUGUI textoConteoIngredientes;
    public int minimoIngredientesParaAvanzar = 3;
    public GameObject botonIrLaboratorio;

    void Update()
    {
        // Actualizar conteo en escena de recolección
        if (textoConteoIngredientes != null)
        {
            int total = GameManager.Instance.TotalIngredientes();
            textoConteoIngredientes.text = $"Ingredientes: {total}";

            if (botonIrLaboratorio != null)
                botonIrLaboratorio.SetActive(total >= minimoIngredientesParaAvanzar);
        }

        // Actualizar inventario en laboratorio
        if (textoInventario != null)
            textoInventario.text = ObtenerTextoInventario();
    }

    string ObtenerTextoInventario()
    {
        string txt = "Inventario:\n";
        foreach (var kvp in GameManager.Instance.inventario)
            if (kvp.Value > 0) txt += $"  {kvp.Key}: {kvp.Value}\n";
        return txt;
    }

    public void MostrarMensaje(string msg)
    {
        if (textoMensaje == null) return;
        StopAllCoroutines();
        textoMensaje.text = msg;
        StartCoroutine(LimpiarMensaje(2.5f));
    }

    IEnumerator LimpiarMensaje(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (textoMensaje != null) textoMensaje.text = "";
    }

    public void ActualizarReceta(RecetaData receta)
    {
        if (textoRecetaActual != null)
        {
            string txt = $"📜 Receta: {receta.nombre}\n";
            foreach (var obj in receta.objetivos)
                txt += $"  - {obj.ingrediente} x{obj.cantidad}\n";
            textoRecetaActual.text = txt;
        }
    }

    public void ActualizarProgreso(Dictionary<string, int> entregados, RecetaData receta)
    {
        if (textoProgreso == null || receta == null) return;
        string txt = "Progreso:\n";
        foreach (var obj in receta.objetivos)
        {
            int entregado = entregados.ContainsKey(obj.ingrediente) ? entregados[obj.ingrediente] : 0;
            txt += $"  {obj.ingrediente}: {entregado}/{obj.cantidad}\n";
        }
        textoProgreso.text = txt;
    }

    public void MostrarVictoria()
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
    }

    // Botones de navegación
    public void IrAlLaboratorio() => SceneManager.LoadScene("Laboratorio");
    public void IrAlMenu()
    {
        GameManager.Instance.ResetearJuego();
        SceneManager.LoadScene("MenuPrincipal");
    }
    public void IrARecoleccion() => SceneManager.LoadScene("Bosque");
}