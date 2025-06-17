using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;
using UnityEngine.EventSystems;


public class FruitSpawner : MonoBehaviour {
    public static FruitSpawner Instance { get; private set; } 
    public GameObject[] fruitPrefabs;
    public Transform spawnPoint;
    public GameObject bombaPrefab;
    private GameObject currentFruit;
    private bool fruitIsFalling = false;
    private int cantidadBomba;

    public float minX = -1f;
    public float maxX = 1f;
    public float fixedY = 4f; 
    public TextMeshProUGUI moneyTxt;
    public TextMeshProUGUI puntuacionActualTxt;

    private bool oroActivo = false;
    private float duracionOro = 30f;
    public UnityEngine.UI.Button botonOro;

    public int dineroInicial;
    public int score = 0;
    public int money = 0;
    public TextMeshProUGUI bombaTxt;
    public TextMeshProUGUI deleteTxt;
    public TextMeshProUGUI oroTxt;
    public GameObject GameManager;

    public float tiempoEsperaBomba = 10f;
    public UnityEngine.UI.Button botonBomba;              

    void Awake() {
        
        if ( Instance != null ) {
            Destroy(gameObject); 
            return;
        }

        // Asignamos la instancia
        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    void Start() {
        score = 0;
        puntuacionActualTxt.text = "0";

        try {
            string moneyStr = Android.GetStringExtra("money");
            string inventarioJson = Android.GetStringExtra("inventory");
            string record = Android.GetStringExtra("record");
            string token = Android.GetStringExtra("token");
            GameData.Instance.token = token;
            if ( string.IsNullOrEmpty(bombaTxt.text) ) {
                ActualizarTextoInventarioDesdeGameData();
            }
            if ( !string.IsNullOrEmpty(inventarioJson) ) {
                string wrappedJson = "{\"items\":" + inventarioJson + "}";
                InventoryWrapper wrapper = JsonUtility.FromJson<InventoryWrapper>(wrappedJson);

                if ( wrapper != null && wrapper.items != null ) {
                    InventoryItem[] items = wrapper.items;

                    if ( !GameData.Instance.inventarioInicializado ) {
                        if ( items.Length > 0 ) {
                            bombaTxt.text = items[0].cantidad.ToString();
                            GameData.Instance.AñadirItem(GameData.BOMBA_ID , items[0].cantidad);
                        }
                        if ( items.Length > 1 ) {
                            deleteTxt.text = items[1].cantidad.ToString();
                            GameData.Instance.AñadirItem(GameData.DELETE_ID , items[1].cantidad);
                        }
                        if ( items.Length > 2 ) {
                            oroTxt.text = items[2].cantidad.ToString();
                            GameData.Instance.AñadirItem(GameData.ORO_ID , items[2].cantidad);
                        }

                        GameData.Instance.inventarioInicializado = true; 
                    }

                    // Actualizar la UI siempre con los valores actuales
                    bombaTxt.text = GameData.Instance.ObtenerCantidadItem(GameData.BOMBA_ID).ToString();
                    deleteTxt.text = GameData.Instance.ObtenerCantidadItem(GameData.DELETE_ID).ToString();
                    oroTxt.text = GameData.Instance.ObtenerCantidadItem(GameData.ORO_ID).ToString();
                }
            }


            int parsedMoney;
            if ( int.TryParse(moneyStr , out parsedMoney) ) {
                this.money = parsedMoney;
                dineroInicial = parsedMoney;
            }
            else {
                Debug.LogWarning("No se pudo convertir 'money' a entero. Valor recibido: " + moneyStr);
                this.money = 0;
            }

            if ( int.TryParse(record , out int r) ) {
                GameData.Instance.record = r;
            }
        }
        catch ( System.Exception e ) {
            Debug.LogError("Error obteniendo datos Android: " + e.Message);
            this.money = 0;
        }


        moneyTxt.text = this.money.ToString();


        SpawnNewFruit();
    }






    void Update() {
        // Si tocamos UI, no hacemos nada
        if ( EventSystem.current.IsPointerOverGameObject() ) return;


        if ( DeleteModeManager.Instance.HandleDeleteClick() ) return;

        if ( !fruitIsFalling && currentFruit != null ) {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float clampedX = Mathf.Clamp(mouseWorldPosition.x , minX , maxX);
            float clampedY = fixedY;

            currentFruit.transform.position = new Vector3(clampedX , clampedY , 0f);

            if ( Input.GetMouseButtonDown(0) ) {
                float mouseY = Input.mousePosition.y;
                float screenHeight = Screen.height;
                float maxValidY = screenHeight * 0.7f;

                if ( mouseY <= maxValidY ) {
                    Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
                    rb.gravityScale = 1f;
                    fruitIsFalling = true;
                    Invoke(nameof(SpawnNewFruit) , 0.5f);
                }
            }
        }
    }



    void SpawnNewFruit() {

        int index = UnityEngine.Random.Range(0 , 3); // Elegir una fruta aleatoria

        currentFruit = Instantiate(fruitPrefabs[index] , spawnPoint.position , Quaternion.identity);
        Fruit fruitScript = currentFruit.GetComponent<Fruit>();
        fruitScript.fruitIndex = index; // Asignar el índice de la fruta

        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Desactivamos gravedad para que no caiga inmediatamente
        rb.linearVelocity = Vector2.zero; // Detener cualquier movimiento
        rb.angularVelocity = 0f; // Detener rotación

        fruitIsFalling = false;
    }

    public void AddScore(int points) {
        score += points;
        puntuacionActualTxt.text = score.ToString();
    }

    public void AddMoney(int amount) {
        if ( oroActivo ) {
            amount *= 2;
        }

        money += amount;
        moneyTxt.text = money.ToString();
    }

    public void TirarBomba() {
        if ( GameData.Instance.ObtenerCantidadItem(GameData.BOMBA_ID) > 0 ) {
            // Desactivar el botón temporalmente
            botonBomba.interactable = false;
            Debug.Log("SE HA DESACTIVAO");
            Vector3 centroContenedor = new Vector3(-0.5f , 4f , 0f);
            GameObject bomba = Instantiate(bombaPrefab , centroContenedor , Quaternion.identity);
            Rigidbody2D rb = bomba.GetComponent<Rigidbody2D>();
            if ( rb != null ) rb.gravityScale = 1f;

            UsarObjeto(GameData.BOMBA_ID , bombaTxt);
            Debug.Log("SE VA A LLAMAR AL COROUTINE");
            // Reactivar después del tiempo de espera
            StartCoroutine(ReactivarBotonBomba());
        }
        else {
            GameManager.GetComponent<GameManager>().ShowAlert();
        }
    }

    private IEnumerator ReactivarBotonBomba() {
        Debug.Log("Coroutine iniciada para reactivar botón");
        yield return new WaitForSeconds(tiempoEsperaBomba);
        botonBomba.interactable = true;
        Debug.Log("Botón reactivado");
    }


    public void UsarObjeto(int idObjeto , TextMeshProUGUI textoUI) {
        Debug.Log("Entrando a UsarObjeto con id: " + idObjeto);
        bool consumido = GameData.Instance.UsarItem(idObjeto);
        Debug.Log("Consumido: " + consumido);
        if ( consumido ) {
            textoUI.text = GameData.Instance.ObtenerCantidadItem(idObjeto).ToString();
            ApiClient.Instance.ConsumirItem(idObjeto , GameData.Instance.token);
        }
        else {
            Debug.Log("No se pudo consumir el objeto");
            GameManager.GetComponent<GameManager>().ShowAlert();
        }
        Debug.Log("Saliendo de UsarObjeto");
    }

    public void ActivarOro() {
        if ( oroActivo ) {
            GameManager.GetComponent<GameManager>().ShowAlert(); // Ya está activo
            return;
        }

        if ( GameData.Instance.ObtenerCantidadItem(GameData.ORO_ID) <= 0 ) {
            GameManager.GetComponent<GameManager>().ShowAlert(); // No tiene oro
            return;
        }

        // Consumir oro y desactivar botón
        UsarObjeto(GameData.ORO_ID , oroTxt); // Actualiza cantidad
        oroActivo = true;
        botonOro.interactable = false;

        Debug.Log("Oro activado: se multiplicarán las ganancias por 2 durante 30 segundos");

        StartCoroutine(TemporizadorOro());
    }

    private IEnumerator TemporizadorOro() {
        yield return new WaitForSeconds(duracionOro);
        oroActivo = false;
        botonOro.interactable = true;
        Debug.Log("Efecto de oro terminado");
    }

    private void ActualizarTextoInventarioDesdeGameData() {
        bombaTxt.text = GameData.Instance.ObtenerCantidadItem(GameData.BOMBA_ID).ToString();
        deleteTxt.text = GameData.Instance.ObtenerCantidadItem(GameData.DELETE_ID).ToString();
        oroTxt.text = GameData.Instance.ObtenerCantidadItem(GameData.ORO_ID).ToString();
    }

}
