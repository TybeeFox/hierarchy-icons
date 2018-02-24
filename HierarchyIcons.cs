using UnityEngine;
using UnityEditor;

namespace Vulpes
{
    [InitializeOnLoad]
    public sealed class HierarchyIcons
    {
        public static IconStyle iconStyle = IconStyle.AssetPreview;
        public enum IconStyle
        {
            None = 0,
            Gizmo = 1,
            AssetPreview = 2,
        }

        public static IconAlignment iconAlignment = IconAlignment.Left;
        public enum IconAlignment
        {
            Left = 0,
            Right = 1,
        }

        static HierarchyIcons()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
            iconStyle = (IconStyle)EditorPrefs.GetInt("HierarchyIconStyle", 2);
            iconAlignment = (IconAlignment)EditorPrefs.GetInt("HierarchyIconAlignment", 0);
        }

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            if (iconStyle == IconStyle.None)
            {
                return;
            }
            GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            float posX = (iconAlignment == IconAlignment.Left) ? 0 : Screen.width - 32;
            if (iconStyle == IconStyle.Gizmo)
            {
                Texture gizmoIcon = EditorGUIUtility.ObjectContent(go, typeof(GameObject)).image;
                if (go && gizmoIcon)
                {
                    GUI.DrawTexture(new Rect(posX, selectionRect.y, 16, 16), gizmoIcon);
                }
            } else if (iconStyle == IconStyle.AssetPreview)
            {
                Texture previewIcon = AssetPreview.GetAssetPreview(PrefabUtility.GetPrefabParent(go));
                if (go && previewIcon)
                {
                    GUI.DrawTexture(new Rect(posX, selectionRect.y, 16, 16), previewIcon);
                }
            }
        }

        [PreferenceItem("Hierarchy")]
        public static void PreferencesGUI()
        {
            iconStyle = (IconStyle)EditorPrefs.GetInt("HierarchyIconStyle", 2);
            iconAlignment = (IconAlignment)EditorPrefs.GetInt("HierarchyIconAlignment", 0);
            iconStyle = (IconStyle)EditorGUILayout.EnumPopup("Icon Style", iconStyle);
            iconAlignment = (IconAlignment)EditorGUILayout.EnumPopup("Icon Alignment", iconAlignment);
            if (GUI.changed)
            {
                EditorPrefs.SetInt("HierarchyIconStyle", (int)iconStyle);
                EditorPrefs.SetInt("HierarchyIconAlignment", (int)iconAlignment);
            }
        }
    }
}