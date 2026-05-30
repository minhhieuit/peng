using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetMyEnrollments;

public record GetMyEnrollmentsQuery(Guid MemberId) : IQuery<List<EnrollmentDto>>;
