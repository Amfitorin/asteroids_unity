using System;
using Core.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneLinkPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var sceneObject = GetSceneObject(property.stringValue);
                var scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
                if (scene == null)
                {
                    property.stringValue = "";
                }
                else if (scene.name != property.stringValue)
                {
                    var sceneObj = GetSceneObject(scene.name);
                    if (sceneObj != null) property.stringValue = scene.name;
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
            }
        }

        private SceneAsset GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName)) return null;

            foreach (var editorScene in EditorBuildSettings.scenes)
                if (editorScene.path.IndexOf(sceneObjectName, StringComparison.Ordinal) != -1)
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;

            Debug.LogWarning(
                $"Scene [{sceneObjectName}] cannot find in build settings");
            return null;
        }
    }
}