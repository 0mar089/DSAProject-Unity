using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitIndex; // Nivel de la fruta, de 0 a N

    private bool merged = false;

    public bool CanMergeWith(Fruit other) {
        return !merged && !other.merged && fruitIndex == other.fruitIndex;
    }

    public void Merge() {
        // Comprobamos si la fruta es del nivel m�ximo (�ltimo �ndice del array de frutas)
        if ( fruitIndex == 10) {
            // Si la fruta est� en el nivel m�ximo, no la eliminamos
            Debug.Log("Fruta de nivel m�ximo, no se puede eliminar.");
            return;  // Sale del m�todo sin destruir la fruta
        }

        merged = true;  // Marca la fruta como fusionada
        Destroy(gameObject);  // Elimina la fruta despu�s de fusionarse, solo si no es de nivel m�ximo
    }


    public void ResetMerge() {
        merged = false;  // Permite que la fruta vuelva a fusionarse
    }
}
