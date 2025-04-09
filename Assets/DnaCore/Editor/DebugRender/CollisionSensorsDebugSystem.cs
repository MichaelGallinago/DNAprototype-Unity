using DnaCore.PhysicsEcs2D.Tiles.Collision;
using Unity.Collections;
using Unity.Entities;
using DnaCore.Utilities.Mathematics;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Collections.NativeArrayOptions;

namespace DnaCore.Editor.DebugRender
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class CollisionSensorsDebugSystem : SystemBase
    {
        private readonly CollisionSensorsDebugSystemAssets _assets = CollisionSensorsDebugSystemAssets.Instance;
        
        private const int GraphicsBufferStride = sizeof(float) * 4;
        private const int BatchLimit = 1023;
        
        private NativeArray<Matrix4x4> _matrices;
        private NativeArray<Color> _colors;
        private GraphicsBuffer _colorBuffer;
        private MaterialPropertyBlock _matProps;
        private RenderParams _renderParams;
        private EntityQuery _tileSensorQuery;
        private int _capacity = 16;
        
        protected override void OnCreate()
        {
            _tileSensorQuery = GetEntityQuery(ComponentType.ReadOnly<TileSensor>());
            AllocateBuffers(_capacity);

            _matProps = new MaterialPropertyBlock();
            _renderParams = new RenderParams(_assets.Material)
            {
                matProps = _matProps, 
                layer = LayerMask.NameToLayer("CollisionGeneration")
            };

            RequireForUpdate<TileSensor>();
        }
        
        protected override void OnUpdate()
        {
            int count = _tileSensorQuery.CalculateEntityCount();
            EnsureCapacity(count);
            FillDebugData();

            _colorBuffer.SetData(_colors);
            _matProps.SetBuffer(DebugRenderShaderProperties.InstanceColorBuffer, _colorBuffer);

            RenderAll(count);
        }
        
        private void EnsureCapacity(int required)
        {
            if (required <= _capacity) return;

            DisposeBuffers();
            AllocateBuffers(_capacity = MathUtilities.NextPowerOfTwo(required));
        }

        private void AllocateBuffers(int size)
        {
            _matrices = new NativeArray<Matrix4x4>(size, Allocator.Persistent, UninitializedMemory);
            _colors = new NativeArray<Color>(size, Allocator.Persistent, UninitializedMemory);
            _colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, size, GraphicsBufferStride);
        }
        
        private void DisposeBuffers()
        {
            if (_matrices.IsCreated) _matrices.Dispose();
            if (_colors.IsCreated) _colors.Dispose();
            _colorBuffer?.Dispose();
        }
        
        private void FillDebugData() => new FillSensorDebugJob
        {
            Matrices = _matrices,
            Colors = _colors
        }.ScheduleParallel(Dependency).Complete();
        
        private void RenderAll(int count)
        {
            var start = 0;

            while (count > 0)
            {
                int batchCount = math.min(BatchLimit, count);
                Graphics.RenderMeshInstanced(_renderParams, _assets.Mesh, 0, _matrices, batchCount, start);
                start += batchCount;
                count -= batchCount;
            }
        }

        protected override void OnDestroy() => DisposeBuffers();
    }
    
    [BurstCompile]
    public partial struct FillSensorDebugJob : IJobEntity
    {
        public NativeArray<Matrix4x4> Matrices;
        public NativeArray<Color> Colors;

        private void Execute([EntityIndexInQuery] int index, in TileSensor sensor)
        {
            Matrices[index] = MatrixUtilities.Get(sensor.Position);
            Colors[index] = sensor.IsInside ? Color.red : Color.green;
        }
    }
}
