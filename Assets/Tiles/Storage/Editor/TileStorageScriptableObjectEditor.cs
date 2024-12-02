using UnityEditor;
using UnityEditor.U2D;
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
            CreateSpriteAtlasAssetButton();
            CheckAtlasSpritesCountButton();
        }

        private void AddTestTileButton()
        {
            if (!GUILayout.Button("Add Test Tile")) return;
            
            var testTile = new BitTile();
            for (uint i = 0; i < TileUtilities.Size; i++)
            {
                testTile[i, i / 2] = true;
            }
                
            _tileStorage.Add(ref testTile);
        }
        
        private void ClearDataButton()
        {
            if (!GUILayout.Button("Clear Test Data")) return;
            _tileStorage.Clear();
        }
        
        private void CreateSpriteAtlasAssetButton()
        {
            if (!GUILayout.Button("Create Sprite Atlas Asset")) return;
            //_tileStorage.CreateSpriteAtlasAsset();
        }
        
        private void CheckAtlasSpritesCountButton()
        {
            if (!GUILayout.Button("Check Atlas Sprites Count")) return;
            Debug.Log($"{_tileStorage.Atlas.GetPackables().Length}/{_tileStorage.Atlas.spriteCount}");
        }
    }
}
