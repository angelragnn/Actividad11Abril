using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonIngrediente : MonoBehaviour
{
    public Button boton;
    public Image iconoImagen;
    public TextMeshProUGUI nombreTexto;
    public TextMeshProUGUI cantidadTexto;

    
    public string nombreIngrediente;

    void Awake()
    {
        if (boton == null) boton = GetComponent<Button>();
        if (iconoImagen == null) iconoImagen = GetComponent<Image>();
        
        if (nombreTexto == null) nombreTexto = GetComponentInChildren<TextMeshProUGUI>();
        
        
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
