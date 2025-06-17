using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static GameData Instance;

    public int record;
    public int dineroAcumulado;
    public string token;

    public const int BOMBA_ID = 1;
    public const int DELETE_ID = 2;
    public const int ORO_ID = 3;

    public bool inventarioInicializado = false;


    public List<InventoryItem> inventario = new List<InventoryItem>();

    void Awake() {
        if ( Instance != null ) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre escenas
    }

    public void AñadirDinero(int cantidad) {
        dineroAcumulado += cantidad;
    }

    public void EstablecerRecordSiEsMayor(int nuevoScore) {
        if ( nuevoScore > record ) {
            record = nuevoScore;
        }
    }

    public void AñadirItem(int itemId , int cantidad) {
        InventoryItem item = inventario.Find(i => i.id == itemId);
        if ( item != null ) {
            item.cantidad += cantidad;
        }
        else {
            inventario.Add(new InventoryItem {
                id = itemId ,
                cantidad = cantidad ,
                nombre = "" ,
                descripcion = "" ,
                url_icon = ""
            });
        }
    }

    public bool UsarItem(int itemId) {
        InventoryItem item = inventario.Find(i => i.id == itemId);
        if ( item != null && item.cantidad > 0 ) {
            item.cantidad--;
            return true;
        }
        return false;
    }

    public int ObtenerCantidadItem(int itemId) {
        InventoryItem item = inventario.Find(i => i.id == itemId);
        return item != null ? item.cantidad : 0;
    }
}
