using System;
using System.Reflection;
using Core.Utils.Extensions;
using CoreMechanics.ObjectLinks;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
  [CustomPropertyDrawer(typeof(GameObjectLink))]
  [CustomPropertyDrawer(typeof(SpriteLink))]
	public class ObjectLinkPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var pathProp = property.FindPropertyRelative("_path");

			var field = fieldInfo;
			var fieldType = field.FieldType;
			if (fieldType.IsArray)
				fieldType = fieldType.GetElementType();

			Type objectType = null;
			if (typeof(IObjectLink<>).IsAssignableFromGeneric(fieldType, out var arguments))
				objectType = arguments[0];

			if (pathProp == null ||
			    property.propertyType != SerializedPropertyType.Generic ||
			    objectType == null)
			{
				EditorGUI.HelpBox(position, "Incorrect using of object attribute for property " + label.text,
					MessageType.Error);
				return;
			}

			if (typeof(Component).IsAssignableFrom(objectType))
				objectType = typeof(GameObject);

			var obj = AssetDatabase.LoadAssetAtPath(pathProp.stringValue, objectType);

			SetTooltip(fieldInfo, label);

			EditorGUI.BeginProperty(position, label, property);
			EditorGUI.BeginChangeCheck();
			position.height = EditorGUI.GetPropertyHeight(SerializedPropertyType.ObjectReference, label);
			
			obj = EditorGUI.ObjectField(position, label, obj, objectType, false);

			position.y += position.height;
			if (EditorGUI.EndChangeCheck())
			{
				var path = AssetDatabase.GetAssetPath(obj);
				pathProp.stringValue = path;
			}

			ResetTooltip(label);
			EditorGUI.EndProperty();
		}

		private static void SetTooltip(FieldInfo fieldInfo, GUIContent label, string defaultTooltip = null)
		{
			if (fieldInfo.HasAttribute<TooltipAttribute>())
			{
				var tts = fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true);
				if (tts.Length > 0)
				{
					label.tooltip = ((TooltipAttribute)tts[0]).tooltip;
					return;
				}
			}

			if (!string.IsNullOrEmpty(defaultTooltip))
				label.tooltip = defaultTooltip;
			else
				label.tooltip = "";
		}

		private static void ResetTooltip(GUIContent label)
		{
			label.tooltip = "";
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var baseHeight = base.GetPropertyHeight(property, label);

			return baseHeight;
		}
	}
}