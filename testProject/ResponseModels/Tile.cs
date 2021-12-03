using testProject.ResponseModels;

namespace testProject
{
    public record Tile(int id, Direction direction, int x, int y)
    {
        public virtual bool Equals(Tile other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return id == other.id;
        }

        public override int GetHashCode()
        {
            return id;
        }

        public override string ToString()
        {
            return id.ToString();
        }
    }
}