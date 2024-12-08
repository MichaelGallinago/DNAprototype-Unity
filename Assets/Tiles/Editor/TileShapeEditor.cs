using Tiles.SolidTypes;
using Tiles.Storage;
using UnityEditor;
using UnityEngine;

namespace Tiles.Editor
{
    [CustomEditor(typeof(TileShape))]
    public class TileShapeEditor : UnityEditor.Editor
    {
        [SerializeField] private TileStorageScriptableObject _tileStorage;
        [SerializeField] private SolidTypesScriptableObject _solidTypes;
        
        private TileShape _tileShape;
        private EditorSnapSwitcher _snapSwitcher;

        private void Awake()
        {
            _tileShape = (TileShape)target;
            UpdateColor();
            UpdateMaterial();
        }

        private void OnEnable()
        {
            _snapSwitcher = new EditorSnapSwitcher(Tools.current);
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        private void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;
        private void DuringSceneGUI(SceneView sceneView) => _snapSwitcher.UpdateTool();
        
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (!EditorGUI.EndChangeCheck()) return;
            
            UpdateColor();
            UpdateMaterial();
        }
        
        private void UpdateColor() => _tileShape.Controller.spriteShapeRenderer.color = _tileShape.SolidType.ToColor();
        
        private void UpdateMaterial() => _tileShape.Collider.sharedMaterial = _solidTypes[_tileShape.SolidType];
    }
}
