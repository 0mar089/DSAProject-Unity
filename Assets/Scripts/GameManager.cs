using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public float TimeTillGameOver = 1.5f;
    [SerializeField] private GameObject gameOverMenuPanel;
    public GameObject alertPanel;
    public GameObject alertPanel2;
    public GameObject alertPanel3;
    public GameObject alertPanel4;
    private bool isGameOver = false;

    private void Awake() {
        instance = this;
    }

    public void GameOver() {
        Debug.Log("HAS PERDIDO");

        if ( isGameOver ) return;
        isGameOver = true;

        GameData.Instance.EstablecerRecordSiEsMayor(FruitSpawner.Instance.score);

        int dineroGanado = FruitSpawner.Instance.money - FruitSpawner.Instance.dineroInicial;
        if ( dineroGanado < 0 ) dineroGanado = 0; // por si acaso
        GameData.Instance.AñadirDinero(dineroGanado);

        Time.timeScale = 0f;
        gameOverMenuPanel.SetActive(true);
    }

    public void OnRestartButton() {
        Time.timeScale = 1f;
        gameOverMenuPanel.SetActive(false);

        

        Destroy(FruitSpawner.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowAlert() {
        alertPanel.SetActive(true);
    }

    public void HideAlert() {
        alertPanel.SetActive(false);
    }

    public void ShowAlert2() {
        alertPanel2.SetActive(true);
    }

    public void HideAlert2() {
        alertPanel2.SetActive(false);
    }

    public void ShowAlert3() {
        alertPanel3.SetActive(true);
    }

    public void ShowAlert4() {
        alertPanel4.SetActive(true);
    }
}
