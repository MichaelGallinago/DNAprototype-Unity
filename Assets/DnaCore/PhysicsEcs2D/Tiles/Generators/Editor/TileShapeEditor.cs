using DnaCore.PhysicsEcs2D.Tiles.SolidTypes;
using UnityEditor;
using UnityEngine;

namespace DnaCore.PhysicsEcs2D.Tiles.Generators.Editor
{
    [CustomEditor(typeof(TileShape))]
    public class TileShapeEditor : UnityEditor.Editor
    {
        [SerializeField] private SolidTypesScriptableObject _solidTypes;
        
        private TileShape _tileShape;
        private ShapeGrid _shapeGrid;
        private EditorSnapSwitcher _snapSwitcher;

        private void Awake()
        {
            _tileShape = (TileShape)target;
            UpdateColor();
            UpdateMaterial();
        }
        
        private void OnEnable()
        {
            _tileShape = (TileShape)target;
            _shapeGrid = _tileShape.GetComponentInParent<ShapeGrid>();
            _shapeGrid?.SetVisibility(true);
            _snapSwitcher = new EditorSnapSwitcher(Tools.current);
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        private void OnDisable()
        {
            _shapeGrid?.SetVisibility(false);
            _snapSwitcher.Disable();
            SceneView.duringSceneGui -= DuringSceneGUI;
        }

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
