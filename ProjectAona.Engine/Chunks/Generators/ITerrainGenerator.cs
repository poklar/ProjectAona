namespace ProjectAona.Engine.Chunks.Generators
{
    /// <summary>
    /// Terrain generator interface.
    /// </summary>
    public interface ITerrainGenerator
    {
        /// <summary>
        /// Builds the chunk.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        void BuildChunk(Chunk chunk);
    }
}
