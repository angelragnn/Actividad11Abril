using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public int escenaDestino = 1;
    public int minimoIngredientes = 0; // 0 = sin requisito

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (minimoIngredientes > 0)
        {
            int total = GameManager.Instance.TotalIngredientes();
            if (total < minimoIngredientes)
            {
                UIManager ui = FindObjectOfType<UIManager>();
                if (ui != null)
                    ui.MostrarMensaje($"Necesitas {minimoIngredientes} ingredientes! Tienes {total}");
                return;
            }
        }

        SceneManager.LoadScene(escenaDestino);
    }
}