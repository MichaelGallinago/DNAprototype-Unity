using Tiles.SolidTypes;
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
            AddTestTileButton();
            ClearDataButton();
        }

        private void AddTestTileButton()
        {
            if (!GUILayout.Button("Add Test Tile")) return;
            
            var testTile = new BitTile();
            for (uint i = 0; i < TileUtilities.Size; i++)
            {
                testTile[i, i / 2] = true;
            }
                
            _tileStorage.AddOrReplace(ref testTile, SolidType.Full);
        }
        
        private void ClearDataButton()
        {
            if (!GUILayout.Button("Clear All Data")) return;
            _tileStorage.Clear();
        }
    }
}
