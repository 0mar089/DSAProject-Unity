using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteModeManager : MonoBehaviour {
    public static DeleteModeManager Instance { get; private set; }
    public GameManager gameManager;
    public bool deleteActive = false;

    public LayerMask fruitLayer;
    public GameObject explosionEffectPrefab; // Animación humo o explosión

    public TMPro.TextMeshProUGUI deleteIndicatorTxt; // Texto o icono para avisar que está activo
    public TextMeshProUGUI deleteTxt;

    private void Awake() {
        if ( Instance != null ) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update() {
        if ( !deleteActive ) return;

        if ( EventSystem.current.IsPointerOverGameObject() ) return;

        if ( Input.GetMouseButtonDown(0) ) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos2D = new Vector2(mousePos.x , mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(pos2D , Vector2.zero , 0f , fruitLayer);
            if ( hit.collider != null ) {
                Fruit fruit = hit.collider.GetComponent<Fruit>();
                if ( fruit != null ) {
                   FruitSpawner.Instance.UsarObjeto(GameData.DELETE_ID, deleteTxt);
                   
                    // Instanciar animación explosión/humo en la posición de la bola
                    if ( explosionEffectPrefab != null ) {
                        GameObject effect = Instantiate(explosionEffectPrefab , fruit.transform.position , Quaternion.identity);
                        Destroy(effect , 1);  
                    }

                    //int puntos = 10 * ( fruit.fruitIndex + 1 ); // Reusa cálculo puntos
                    //FruitSpawner.Instance.AddScore(puntos);
                    //FruitSpawner.Instance.AddMoney(puntos / 20);

                    // Destruir bola tras 1 segundo para que la animación se vea
                    Destroy(fruit.gameObject , 1f);

                    DeactivateDeleteMode();
                }
            }
        }
    }

    public bool IsDeleteModeActive() {
        return deleteActive;
    }

    public void ActivateDeleteMode() {
        if ( GameData.Instance.ObtenerCantidadItem(GameData.DELETE_ID) > 0 ) {
            deleteActive = true;
            UpdateIndicator(true);
        }
        else {
            Debug.Log("No tienes deletes");
            GameManager.instance.ShowAlert2(); 
            deleteActive = false;
            UpdateIndicator(false);
        }
    }

    public void DeactivateDeleteMode() {
        deleteActive = false;
        UpdateIndicator(false);
    }

    private void UpdateIndicator(bool active) {
        if ( deleteIndicatorTxt != null ) {
            deleteIndicatorTxt.gameObject.SetActive(active);
            deleteIndicatorTxt.text = active ? "DELETE ACTIVADO" : "";
        }
    }
    public bool HandleDeleteClick() {
        if ( !deleteActive ) return false;
        if ( EventSystem.current.IsPointerOverGameObject() ) return false;

        if ( Input.GetMouseButtonDown(0) ) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos2D = new Vector2(mousePos.x , mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(pos2D , Vector2.zero , 0f , fruitLayer);
            Debug.Log("Click en: " + pos2D + " - Resultado Raycast: " + ( hit.collider != null ? hit.collider.name : "nada" ));

            if ( hit.collider != null ) {
                Fruit fruit = hit.collider.GetComponent<Fruit>();
                if ( fruit != null ) {
                    Debug.Log("Se va a eliminar fruta: " + fruit.name);
                    FruitSpawner.Instance.UsarObjeto(GameData.DELETE_ID, deleteTxt);
              

                    if ( explosionEffectPrefab != null ) {
                        Instantiate(explosionEffectPrefab , fruit.transform.position , Quaternion.identity);
                    }
                    Destroy(fruit.gameObject , 1f);
                    DeactivateDeleteMode();
                    return true;
                }
            }
        }
        return false;
    }

}
