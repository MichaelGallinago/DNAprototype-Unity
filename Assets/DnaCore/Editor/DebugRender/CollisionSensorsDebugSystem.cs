using Unity.Collections;
using Unity.Entities;
using DnaCore.Utilities.Mathematics;
using UnityEngine;
using static Unity.Collections.NativeArrayOptions;

namespace DnaCore.Editor.DebugRender
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class CollisionSensorsDebugSystem : SystemBase
    {
        private static readonly int InstanceColorBuffer = Shader.PropertyToID("_InstanceColorBuffer");
        
        private readonly CollisionSensorsDebugSystemAssets _assets = CollisionSensorsDebugSystemAssets.Instance;
        
        private NativeArray<Matrix4x4> _matrices;
        private NativeArray<Color> _colors;
        private GraphicsBuffer _colorBuffer;
        private MaterialPropertyBlock _matProps;
        private RenderParams _renderParams;

        private const int Count = 4;
        
        protected override void OnCreate()
        {
            _matrices = new NativeArray<Matrix4x4>(Count, Allocator.Persistent, UninitializedMemory);
            _colors = new NativeArray<Color>(Count, Allocator.Persistent, UninitializedMemory);
            _colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, Count, sizeof(float) * 4);

            _matProps = new MaterialPropertyBlock();
            _renderParams = new RenderParams(_assets.Material) { matProps = _matProps };

            SetData(0, 0, 0, Color.red);
            SetData(1, 16, 16, Color.black);
            SetData(2, -16, 2, Color.green);
            SetData(3, -48, 48, Color.white);
        }
        
        private void SetData(int index, int x, int y, Color color)
        {
            _matrices[index] = MatrixUtilities.Get(x, y);
            _colors[index] = color;
        }
    
        private void SetData(int index, int x, int y, int scaleX, int scaleY, Color color)
        {
            _matrices[index] = MatrixUtilities.Get(x, y, scaleX, scaleY);
            _colors[index] = color;
        }
        
        protected override void OnUpdate()
        {
            _colorBuffer.SetData(_colors);
            _matProps.SetBuffer(InstanceColorBuffer, _colorBuffer);
        
            Graphics.RenderMeshInstanced(_renderParams, _assets.Mesh, 0, _matrices, Count);
        }
        
        protected override void OnDestroy()
        {
            _matrices.Dispose();
            _colors.Dispose();
            _colorBuffer.Dispose();
        }
    }
}
