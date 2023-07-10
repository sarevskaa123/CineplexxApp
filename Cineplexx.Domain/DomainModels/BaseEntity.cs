using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace Cineplexx.Domain.DomainModels
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
