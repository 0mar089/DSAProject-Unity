using UnityEngine;

public class BombBehavior : MonoBehaviour {
    public float explosionRadius = 0.1f;
    public LayerMask fruitLayer;
    public GameObject explosionEffectPrefab; // Prefab de animación de explosión

    private bool hasExploded = false;
    private bool collidedWithFruit = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        if ( hasExploded ) return;

        // Comprobar si la colisión fue con una fruta usando layerMask
        if ( ( ( 1 << collision.gameObject.layer ) & fruitLayer ) != 0 ) {
            collidedWithFruit = true;
        }

        Explode();
    }

    private void Explode() {
        if ( hasExploded ) return;
        hasExploded = true;

        // Instanciar animación de explosión
        if ( explosionEffectPrefab != null ) {
            Instantiate(explosionEffectPrefab , transform.position , Quaternion.identity);
        }

        // Detectar frutas en el radio
        Collider2D[] frutas = Physics2D.OverlapCircleAll(transform.position , explosionRadius , fruitLayer);

        if ( frutas.Length > 0 ) {
            foreach ( Collider2D fruit in frutas ) {
                Fruit fruitScript = fruit.GetComponent<Fruit>();
                if ( fruitScript != null ) {
                    // Sumar puntos y dinero solo si colisionamos con fruta directamente
                    if ( collidedWithFruit ) {
                        int puntos = CalculatePoints(fruitScript.fruitIndex);
                        FruitSpawner.Instance.AddScore(puntos);

                        int dinero = puntos / 20;
                        FruitSpawner.Instance.AddMoney(dinero);
                    }

                    Destroy(fruit.gameObject);
                }
            }
        }

        // Destruir bomba tras delay para ver animación
        Destroy(gameObject , 0.1f);
    }

    // Dibuja el radio de explosión en el editor para debug
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , explosionRadius);
    }

    private int CalculatePoints(int fruitLevel) {
        return 10 * ( fruitLevel + 1 );
    }
}
