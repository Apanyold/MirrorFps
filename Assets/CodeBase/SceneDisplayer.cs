using UnityEngine.SceneManagement;
using UnityEngine;

namespace CodeBase
{
    public class SceneDisplayer : MonoBehaviour
    {
        private void OnGUI()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            var content = new GUIContent(sceneName);

            GUIStyle style = GUI.skin.box;
            style.alignment = TextAnchor.MiddleCenter;

            Vector2 size = style.CalcSize(content);

            float heightOffset = 2.0f * size.y;
            float widthOffset = 2.0f * size.x;

            GUI.Box(new Rect(Screen.width / 2 - widthOffset / 2, Screen.height - heightOffset, widthOffset, heightOffset), sceneName);
        }
    }
}