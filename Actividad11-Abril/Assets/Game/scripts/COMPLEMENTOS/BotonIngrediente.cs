using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonIngrediente : MonoBehaviour
{
    public Button boton;
    public Image iconoImagen;
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI cantidadTexto;

    // Ya no está oculto, puedes escribir el nombre en el Inspector
    public string nombreIngrediente;

    void Awake()
    {
        if (boton == null) boton = GetComponent<Button>();
        if (iconoImagen == null) iconoImagen = GetComponent<Image>();
        // Buscar los textos en los hijos si no se asignaron
        if (nombreTexto == null) nombreTexto = GetComponentInChildren<TextMeshProUGUI>();
        
        // --- NUEVA LÓGICA DE CRAFTEO AUTOMÁTICO ---
        boton.onClick.AddListener(AlHacerClic);
    }

    void AlHacerClic()
    {
        CauldronManager cauldron = FindObjectOfType<CauldronManager>();
        if (cauldron != null)
        {
            cauldron.IntentarEntregar(nombreIngrediente);
        }
        else
        {
            Debug.LogWarning("¡No se encontró ningún Caldero en la escena!");
        }
    }
}
