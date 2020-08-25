using System;

namespace xgca.entity.Models
{
    public class _Model
    {

        public interface IIdentifiableEntity
        {
            Guid Id { get; set; }
        }
        public interface IAuditableEntity : IIdentifiableEntity
        {
            string CreatedBy { get; set; }
            DateTime CreatedOn { get; set; }
            string UpdatedBy { get; set; }
            DateTime UpdatedOn { get; set; }
        }

        public interface ISoftDeletableEntity : IAuditableEntity
        {
            int IsDeleted { get; set; }
            string DeletedBy { get; set; }
            DateTime? DeletedOn { get; set; }
        }

        public abstract class IdentifiableEntity : IIdentifiableEntity
        {
            public Guid Id { get; set; }
        }

        public abstract class AuditableEntity : IAuditableEntity
        {
            public Guid Id { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime UpdatedOn { get; set; }
        }

        public abstract class SoftDeletableEntity : ISoftDeletableEntity
        {
            public Guid Id { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime UpdatedOn { get; set; }
            public int IsDeleted { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedOn { get; set; }
        }
    }
}
