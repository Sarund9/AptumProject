using System;
using System.Collections.Generic;
using System.Text;
using Veldrid;
using Veldrid.SPIRV;

namespace AptumEngine.Core
{
    public abstract class VFShaderBase : AObject
    {
        public string Name { get; protected set; }
        public Shader VertexShader { get; protected set; }
        public Shader FragmentShader { get; protected set; }
        public VertexLayoutDescription Layout { get; protected set; }

        public VFShaderBase(ResourceFactory factory, string name)
        {
            Name = name;

            var vertSpirvBytes = Assets.ReadEmbeddedAssetBytes(name + "_v");
            var fragSpirvBytes = Assets.ReadEmbeddedAssetBytes(name + "_f");

            var shaders = factory.CreateFromSpirv(
                new ShaderDescription(ShaderStages.Vertex, vertSpirvBytes, "main"),
                new ShaderDescription(ShaderStages.Fragment, fragSpirvBytes, "main")
                );

            VertexShader = shaders[0];
            FragmentShader = shaders[1];
        }
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);

            if (VertexShader != null && FragmentShader != null)
            {
                VertexShader.Dispose();
                VertexShader = null;
                FragmentShader.Dispose();
                FragmentShader = null;
            }
        }
    }
    public class ColorShader : VFShaderBase
    {
        public DeviceBuffer ProjectionBuffer;
        public DeviceBuffer WorldBuffer;
        public ResourceSet ResourceSet;
        public ResourceLayout ResourceLayout;

        public ColorShader(ResourceFactory factory) : base(factory, "Color")
        {
            Layout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
                new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));

            ProjectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            WorldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

            ResourceLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("Projection", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("World", ResourceKind.UniformBuffer, ShaderStages.Vertex))
                    );

            ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                ResourceLayout,
                ProjectionBuffer,
                WorldBuffer)
                );
        }
        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);

            ResourceSet.Dispose();
            ResourceSet = null;
            ResourceLayout.Dispose();
            ResourceLayout = null;
            ProjectionBuffer.Dispose();
            ProjectionBuffer = null;
            WorldBuffer.Dispose();
            WorldBuffer = null;
        }
    }
}
