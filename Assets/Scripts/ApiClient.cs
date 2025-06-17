using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class ApiClient : MonoBehaviour {
    public static ApiClient Instance;
    public TextMeshProUGUI respuestaText;

    void Awake() {
        if ( Instance == null ) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void ConsumirItem(int idObjeto , string token) {
        StartCoroutine(ConsumirItemCoroutine(idObjeto , token));
    }

    private IEnumerator ConsumirItemCoroutine(int idObjeto , string token) {
        WWWForm form = new WWWForm();
        form.AddField("idObjeto" , idObjeto);

        UnityWebRequest www = UnityWebRequest.Post("https://dsa2.upc.edu/TocaBolas/Shop/consumir", form);
        www.SetRequestHeader("Authorization" , "Bearer " + token);

        yield return www.SendWebRequest();

        if ( www.result != UnityWebRequest.Result.Success ) {
            Debug.LogError("Error al consumir ítem: " + www.error);
        }
        else {
            Debug.Log("Ítem consumido correctamente: " + www.downloadHandler.text);
            respuestaText.text = www.error + ":" + www.downloadHandler.text;
        }
    }

    public void EnviarDineroYRecord(int dinero , int record , string token , System.Action onSuccess) {
        StartCoroutine(EnviarDatosCoroutine(dinero , record , token , onSuccess));
    }

    private IEnumerator EnviarDatosCoroutine(int dinero , int record , string token , System.Action onSuccess) {
        // Enviar dinero
        WWWForm formDinero = new WWWForm();
        formDinero.AddField("cantidad" , dinero);

        UnityWebRequest reqDinero = UnityWebRequest.Post("https://dsa2.upc.edu/TocaBolas/anadirDinero" , formDinero);
        reqDinero.SetRequestHeader("Authorization" , "Bearer " + token);
        yield return reqDinero.SendWebRequest();

        if ( reqDinero.result != UnityWebRequest.Result.Success ) {
            Debug.LogError("Error al enviar dinero: " + reqDinero.error);
            yield break;
        }

        // Enviar récord
        WWWForm formRecord = new WWWForm();
        formRecord.AddField("puntuacion" , record);

        UnityWebRequest reqRecord = UnityWebRequest.Post("https://dsa2.upc.edu/TocaBolas/actualizarPuntuacion" , formRecord);
        reqRecord.SetRequestHeader("Authorization" , "Bearer " + token);
        yield return reqRecord.SendWebRequest();

        if ( reqRecord.result != UnityWebRequest.Result.Success ) {
            Debug.LogError("Error al enviar récord: " + reqRecord.error);
            yield break;
        }

        Debug.Log("Dinero y récord enviados correctamente");
        onSuccess?.Invoke(); // Llama a la acción de éxito
    }

}
