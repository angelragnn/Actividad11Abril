using UnityEngine;
using UnityEngine.SceneManagement;

public class EntradaLaboratorio : MonoBehaviour
{
    public int minimoIngredientes = 3;
    public int escenaDestino = 2;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        int total = GameManager.Instance.TotalIngredientes();

        if (total >= minimoIngredientes)
        {
            SceneManager.LoadScene(escenaDestino);
        }
        else
        {
            
            
            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null)
                ui.MostrarMensaje($"Necesitas {minimoIngredientes} ingredientes! Tienes {total}");
        }
    }
}