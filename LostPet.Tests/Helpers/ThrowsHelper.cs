using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Constraints;

namespace LostPet.Tests.Helpers;

[ExcludeFromCodeCoverage]
public class ThrowsHelper : Throws
{
        public static ExactTypeConstraint DbUpdateException
        {
            get { return TypeOf(typeof(DbUpdateException)); }
        }
}