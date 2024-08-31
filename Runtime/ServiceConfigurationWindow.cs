using UnityEditor;
using UnityEngine;

namespace JordanTama.ServiceLocator
{
    public class ServiceConfigurationWindow : EditorWindow
    {
        [MenuItem("JordanTama/Service Locator/Configuration")]
        private static void ShowWindow()
        {
            var window = GetWindow<ServiceConfigurationWindow>();
            window.titleContent = new GUIContent("Service Configuration");
            window.Show();
        }

        private void OnGUI()
        {
            
            
            // TODO: Use reflection to locate ALL configurable services
            // TODO: Foreach each service, retrieve their Configuration SO
            // TODO: Draw the Configuration SO in a foldout
        }
    }
}
