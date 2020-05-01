using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : UnityEditor.Editor
    {
        private Planet planet;
        private UnityEditor.Editor shapeEditor;
        private UnityEditor.Editor colourEditor;

        private void OnEnable()
        {
            planet = (Planet) target;
        }

        public override void OnInspectorGUI()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if (check.changed)
                {
                    planet.GeneratePlanet();
                }
            }

            if (GUILayout.Button("Generate Planet"))
            {
                planet.GeneratePlanet();
            }
            
            DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
            DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.colourSettingsFoldout, ref colourEditor);
        }

        private static void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldOut, ref UnityEditor.Editor editor)
        {
            if (settings == null) return;
            foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);
            if (!foldOut) return;

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();

                if (check.changed)
                {
                    onSettingsUpdated?.Invoke();
                }
            }
        }
    }
}