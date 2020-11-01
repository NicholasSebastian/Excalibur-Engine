namespace ExcaliburEngine
{
    public class Model
    {
        public readonly int vertexArrayObject;
        public uint[] indices;
        public readonly Texture texture;

        internal Model(int vertexArrayObject, uint[] indices, Texture texture)
        {
            this.vertexArrayObject = vertexArrayObject;
            this.indices = indices;
            this.texture = texture;
        }
    }
}
