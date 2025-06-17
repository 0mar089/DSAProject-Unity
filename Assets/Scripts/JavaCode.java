import android.app.Activity;
import android.content.ComponentName;
import android.content.Intent;
import com.unity3d.player.UnityPlayer;

public class JavaCode {

    public static void sendResultsAndLaunchActivity(int dinero, int puntuacion, String pkg, String activity) {
        Intent i = new Intent();
        i.setComponent(new ComponentName(pkg, activity));
        i.putExtra("dinero", dinero);
        i.putExtra("puntuacion", puntuacion);

        UnityPlayer.currentActivity.startActivity(i);
        UnityPlayer.currentActivity.finish();
    }
}