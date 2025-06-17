using UnityEngine;

public class FruitCollision : MonoBehaviour {
    private Fruit fruit;

    void Awake() {
        fruit = GetComponent<Fruit>();
    }

    // Detecta cuando la fruta colisiona con otra fruta
    private void OnCollisionEnter2D(Collision2D collision) {
        // Si AMBOS son bombas, salimos inmediatamente
        if ( this.CompareTag("Bomba") && collision.gameObject.CompareTag("Bomba") ) {
            return;
        }

        // Si UNO es bomba y el otro no, tampoco hacer nada (no se suman puntos ni se fusionan)
        if ( this.CompareTag("Bomba") || collision.gameObject.CompareTag("Bomba") ) {
            return;
        }

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();
        if ( otherFruit != null && fruit.CanMergeWith(otherFruit) ) {
            // Instanciar nueva fruta del siguiente nivel
            if ( FruitSpawner.Instance != null ) {
                int newIndex = fruit.fruitIndex + 1;
                if ( newIndex < FruitSpawner.Instance.fruitPrefabs.Length ) {
                    Vector3 spawnPos = transform.position;
                    GameObject newFruitObj = Instantiate(FruitSpawner.Instance.fruitPrefabs[newIndex] , spawnPos , Quaternion.identity);
                    newFruitObj.GetComponent<Rigidbody2D>().gravityScale = 1f;
                    newFruitObj.GetComponent<Fruit>().fruitIndex = newIndex;
                    newFruitObj.GetComponent<Fruit>().ResetMerge();

                    int pointsToAdd = CalculatePoints(newIndex);
                    FruitSpawner.Instance.AddScore(pointsToAdd);

                    int moneyToAdd = pointsToAdd / 20;
                    FruitSpawner.Instance.AddMoney(moneyToAdd);
                }
            }
            else {
                Debug.LogError("FruitSpawner.Instance no está asignado.");
            }

            fruit.Merge(); // Fusiona la fruta actual
            otherFruit.Merge(); // Fusiona la otra fruta
        }
    }


    private int CalculatePoints(int fruitLevel) {
        // puntos = 10 * (nivel + 1)
        return 10 * ( fruitLevel + 1 );
    }
}
