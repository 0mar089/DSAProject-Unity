using UnityEngine;

public class DeleteButtonHandler : MonoBehaviour
{
    public void OnDeleteButtonClicked() {
        DeleteModeManager.Instance.ActivateDeleteMode();
    }
}
