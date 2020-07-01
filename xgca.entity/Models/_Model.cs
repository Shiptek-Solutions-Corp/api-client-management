using System;
using System.Collections.Generic;
using System.Text;

namespace xgca.entity.Models
{
    public class _Model
    {
        public interface IAuditableEntity
        {
            string CreatedBy { get; set; }
            DateTime CreatedOn { get; set; }
            string UpdatedBy { get; set; }
            DateTime UpdatedOn { get; set; }
        }

        public interface ISoftDeletableEntity : IAuditableEntity
        {
            bool IsDeleted { get; set; }
            string DeletedBy { get; set; }
            DateTime? DeletedOn { get; set; }
        }

        public interface IIdentifiableEntity : ISoftDeletableEntity
        {
            Guid Id { get; set; }
        }

        public abstract class AuditableEntity : IAuditableEntity
        {
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime UpdatedOn { get; set; }
        }

        public abstract class SoftDeleteEntity : ISoftDeletableEntity
        {
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime UpdatedOn { get; set; }
            public bool IsDeleted { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedOn { get; set; }
        }

        public abstract class IdentifiableEntity : IIdentifiableEntity
        {
            public string CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public DateTime UpdatedOn { get; set; }
            public bool IsDeleted { get; set; }
            public string DeletedBy { get; set; }
            public DateTime? DeletedOn { get; set; }
            public Guid Id { get; set; }
        }
    }
}
