
using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
   
    [HideInInspector] public string nombreIngrediente;
    [HideInInspector] public int valor;

 
    public void Inicializar(string nombre, int val, Sprite sprite)
    {
        nombreIngrediente = nombre;
        valor = val;
        GetComponent<SpriteRenderer>().sprite = sprite;
        gameObject.name = nombre; 
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        
        GameManager.Instance.AgregarIngrediente(nombreIngrediente);

       
        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null)
            ui.MostrarMensaje($"¡Recogiste: {nombreIngrediente}!");

        
        Destroy(gameObject);
    }
}