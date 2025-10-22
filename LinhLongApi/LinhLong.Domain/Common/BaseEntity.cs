namespace LinhLong.Domain.Common
{
    /// <summary>
    /// Base entity with sequential Guid Id for SQL Server performance.
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }

        protected BaseEntity()
        {
            Id = SequentialGuid.NewGuid();
        }

        public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;
    }
}
