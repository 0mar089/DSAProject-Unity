using System;
using UnityEngine;
using UnityEngine.Windows;

public class Android {
    public static string GetStringExtra(string key) {
#if UNITY_ANDROID
        try {
            Debug.Log("[Android] Intentando obtener extra con key: " + key);
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
            string value = intent.Call<string>("getStringExtra" , key);

            Debug.Log("[Android] Valor obtenido para key '" + key + "': " + value);
            return value;
        }
        catch ( Exception e ) {
            Debug.LogError("[Android] Error al obtener extra: " + e.Message);
            return null;
        }
#else
        Debug.LogWarning("[Android] GetStringExtra llamado en una plataforma no Android.");
        return null;
#endif
    }

    public static void SetResultAndFinish(int dinero , int puntuacion , string pkg , string activity) {
#if UNITY_ANDROID
        try {
            Debug.Log("[Android] Enviando resultados: dinero=" + dinero + ", puntuacion=" + puntuacion);
            Debug.Log("[Android] Lanzando actividad: " + pkg + "/" + activity);

            AndroidJavaClass javacode = new AndroidJavaClass("JavaCode");
            object[] args = { dinero , puntuacion , pkg , activity };
            javacode.CallStatic("sendResultsAndLaunchActivity" , args);

            Debug.Log("[Android] Actividad lanzada y Unity finalizado.");
        }
        catch ( Exception e ) {
            Debug.LogError("[Android] Error al enviar resultados y lanzar actividad: " + e.Message);
        }
#else
        Debug.LogWarning("[Android] SetResultAndFinish llamado en una plataforma no Android.");
#endif
    }
}
