﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyBox
{
    /// <summary>
    /// Apply to MonoBehaviour field to assert that this field is assigned via inspector (not null, false, empty of zero) on playmode
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MustBeAssignedAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.Experimental.SceneManagement;
    using EditorTools;

    [InitializeOnLoad]
    public class MustBeAssignedAttributeChecker
    {
        /// <summary>
        /// A way to conditionally disable MustBeAssigned check
        /// </summary>
        public static Func<FieldInfo, Object, bool> ExcludeFieldFilter;

        static MustBeAssignedAttributeChecker()
        {
            MyEditorEvents.OnSave += AssertComponentsInScene;
            PrefabStage.prefabSaved += AssertComponentsInPrefab;
        }

        private static void AssertComponentsInScene()
        {
#if UNITY_2020_1_OR_NEWER
            var behaviours = Object.FindObjectsOfType<MonoBehaviour>(true);
#else
			var behaviours = Object.FindObjectsOfType<MonoBehaviour>();
#endif
            // ReSharper disable once CoVariantArrayConversion
            AssertComponents(behaviours);

            if (MyBoxSettings.EnableSOCheck)
            {
                var scriptableObjects = MyScriptableObject.LoadAssets<ScriptableObject>();
                // ReSharper disable once CoVariantArrayConversion
                AssertComponents(scriptableObjects);
            }
        }

        private static void AssertComponentsInPrefab(GameObject prefab)
        {
            var components = prefab.GetComponentsInChildren<MonoBehaviour>();
            // ReSharper disable once CoVariantArrayConversion
            AssertComponents(components);
        }

        private static void AssertComponents(Object[] objects)
        {
            var mustBeAssignedType = typeof(MustBeAssignedAttribute);
            foreach (var obj in objects)
            {
                if (obj == null) continue;

                var typeOfScript = obj.GetType();
                var typeFields =
                    typeOfScript.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var field in typeFields)
                {
                    if (!field.IsDefined(mustBeAssignedType, false)) continue;

                    // Used by external systems to exclude specific fields.
                    // Specifically for ConditionalFieldAttribute
                    if (FieldIsExcluded(field, obj)) continue;

                    AssertField(obj, typeOfScript, field);
                }
            }
        }

        private static void AssertField(Object targetObject, Type targetType, FieldInfo field)
        {
            var fieldValue = field.GetValue(targetObject);

            var valueTypeWithDefaultValue = field.FieldType.IsValueType &&
                                            Activator.CreateInstance(field.FieldType).Equals(fieldValue);
            if (valueTypeWithDefaultValue)
            {
                Debug.LogError($"{targetType.Name} caused: {field.Name} is Value Type with default value",
                    targetObject);
                return;
            }


            var nullReferenceType = fieldValue == null || fieldValue.Equals(null);
            if (nullReferenceType)
            {
                Debug.LogError($"{targetType.Name} caused: {field.Name} is not assigned (null value)", targetObject);
                return;
            }


            var emptyString = field.FieldType == typeof(string) && (string) fieldValue == string.Empty;
            if (emptyString)
            {
                Debug.LogError($"{targetType.Name} caused: {field.Name} is not assigned (empty string)", targetObject);
                return;
            }


            var arr = fieldValue as Array;
            var emptyArray = arr != null && arr.Length == 0;
            if (emptyArray)
                Debug.LogError($"{targetType.Name} caused: {field.Name} is not assigned (empty array)", targetObject);
        }

        private static bool FieldIsExcluded(FieldInfo field, Object behaviour)
        {
            if (ExcludeFieldFilter == null) return false;

            foreach (var filterDelegate in ExcludeFieldFilter.GetInvocationList())
            {
                var filter = filterDelegate as Func<FieldInfo, Object, bool>;
                if (filter != null && filter(field, behaviour)) return true;
            }

            return false;
        }
    }
}
#endif