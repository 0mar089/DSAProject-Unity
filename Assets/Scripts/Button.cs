using TMPro;
using UnityEngine;
using System;

public class Button : MonoBehaviour
{

    public TextMeshProUGUI moneyTxt;
    public TextMeshProUGUI puntuacionActualTxt;

    public void clickLobby() {

        Debug.Log("Click en botón, enviando dinero y record...");

        int dinero = GameData.Instance.dineroAcumulado;
        int record = GameData.Instance.record;
        string token = GameData.Instance.token;


        ApiClient.Instance.EnviarDineroYRecord(dinero , record , token , () => {
            // Solo se ejecuta cuando ambas llamadas han sido exitosas
            Android.SetResultAndFinish(100 , 100 , "com.example.loginregister" , "com.example.loginregister.LobbyActivity");
        });

    }



}
