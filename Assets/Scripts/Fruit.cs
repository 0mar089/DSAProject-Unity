using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitIndex; // Nivel de la fruta, de 0 a N

    private bool merged = false;

    public bool CanMergeWith(Fruit other) {
        return !merged && !other.merged && fruitIndex == other.fruitIndex;
    }

    public void Merge() {
        // Comprobamos si la fruta es del nivel máximo (último índice del array de frutas)
        if ( fruitIndex == 10) {
            // Si la fruta está en el nivel máximo, no la eliminamos
            Debug.Log("Fruta de nivel máximo, no se puede eliminar.");
            return;  // Sale del método sin destruir la fruta
        }

        merged = true;  // Marca la fruta como fusionada
        Destroy(gameObject);  // Elimina la fruta después de fusionarse, solo si no es de nivel máximo
    }


    public void ResetMerge() {
        merged = false;  // Permite que la fruta vuelva a fusionarse
    }
}
