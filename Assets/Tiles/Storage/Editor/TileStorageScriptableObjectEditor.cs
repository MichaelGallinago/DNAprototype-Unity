using UnityEditor;
using UnityEngine;

namespace Tiles.Storage.Editor
{
    [CustomEditor(typeof(TileStorageScriptableObject))]
    public class TileStorageScriptableObjectEditor : UnityEditor.Editor
    {
        private TileStorageScriptableObject _tileStorage;

        private void Awake() => _tileStorage = (TileStorageScriptableObject)target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ClearDataButton();
        }
        
        private void ClearDataButton()
        {
            if (!GUILayout.Button("Clear All Data")) return;
            _tileStorage.Clear();
        }
    }
}
