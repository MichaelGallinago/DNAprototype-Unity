using UnityEditor;
using UnityEngine;

namespace Tiles.Editor
{
    public struct EditorSnapSwitcher
    {
        private Tool _lastTool;
        private Vector3 _lastMoveSnap;
        private bool _lastGridSnapEnabled;
        
        public EditorSnapSwitcher(Tool currentTool)
        {
            _lastTool = currentTool;
            _lastMoveSnap = EditorSnapSettings.move;
            _lastGridSnapEnabled = EditorSnapSettings.gridSnapEnabled;
        }

        public void UpdateTool()
        {
            if (Tools.current == _lastTool) return;
            _lastTool = Tools.current;
            
            if (_lastTool != Tool.Custom)
            {
                EditorSnapSettings.move = _lastMoveSnap;
                EditorSnapSettings.gridSnapEnabled = _lastGridSnapEnabled;
                return;
            }

            _lastMoveSnap = EditorSnapSettings.move;
            _lastGridSnapEnabled = EditorSnapSettings.gridSnapEnabled;
            EditorSnapSettings.move = new Vector3(8f, 8f, 8f);
            EditorSnapSettings.gridSnapEnabled = true;
        }
    }
}
