using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD - Escena Recolección")]
    public TextMeshProUGUI textoMensaje;
    public TextMeshProUGUI textoConteoIngredientes;
    public int minimoIngredientesParaAvanzar = 3;
    public GameObject botonIrLaboratorio;

    [Header("HUD - Laboratorio")]
    public TextMeshProUGUI textoInventario;
    public TextMeshProUGUI textoRecetaActual;
    public TextMeshProUGUI textoProgreso;
    public GameObject panelVictoria;

    [Header("Menú Principal")]
    public GameObject panelInstrucciones;

    void Update()
    {
        if (GameManager.Instance == null) return;

        if (textoConteoIngredientes != null)
        {
            int total = GameManager.Instance.TotalIngredientes();
            textoConteoIngredientes.text = $"Ingredientes: {total}";

            if (botonIrLaboratorio != null)
                botonIrLaboratorio.SetActive(total >= minimoIngredientesParaAvanzar);
        }

        if (textoInventario != null)
            textoInventario.text = ObtenerTextoInventario();
    }

    string ObtenerTextoInventario()
    {
        string txt = "Inventario:\n";
        foreach (var kvp in GameManager.Instance.inventario)
            if (kvp.Value > 0)
                txt += $"  {kvp.Key}: {kvp.Value}\n";
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
        if (textoRecetaActual == null) return;
        string txt = $"Receta: {receta.nombre}\n";
        foreach (var obj in receta.objetivos)
            txt += $"  - {obj.ingrediente} x{obj.cantidad}\n";
        textoRecetaActual.text = txt;
    }

    public void ActualizarProgreso(Dictionary<string, int> entregados, RecetaData receta)
    {
        if (textoProgreso == null || receta == null) return;
        string txt = "Progreso:\n";
        foreach (var obj in receta.objetivos)
        {
            int e = entregados.ContainsKey(obj.ingrediente) ? entregados[obj.ingrediente] : 0;
            txt += $"  {obj.ingrediente}: {e}/{obj.cantidad}\n";
        }
        textoProgreso.text = txt;
    }

    public void MostrarVictoria()
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
    }

    public void MostrarInstrucciones()
    {
        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(!panelInstrucciones.activeSelf);
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void IrAlLaboratorio()
    {
        Debug.Log("[UIManager] Yendo al laboratorio...");
        SceneManager.LoadScene(2);
    }

    public void IrARecoleccion()
    {
        SceneManager.LoadScene(1);
    }

    public void IrAlMenu()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetearJuego();
        SceneManager.LoadScene(0);
    }
}