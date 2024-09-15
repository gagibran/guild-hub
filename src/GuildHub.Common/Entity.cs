namespace GuildHub.Common;

public abstract class Entity
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; private set; }

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != obj.GetType())
        {
            return false;
        }
        return Id == ((Entity)obj).Id;
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (left is null && right is null)
        {
            return true;
        }
        if (left is null || right is null)
        {
            return false;
        }
        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public bool Equals(Entity? other)
    {
        return Equals((object?)other);
    }
}
