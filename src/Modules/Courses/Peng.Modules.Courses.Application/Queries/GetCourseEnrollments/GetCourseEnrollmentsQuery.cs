using Peng.Modules.Courses.Application.DTOs;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Courses.Application.Queries.GetCourseEnrollments;

public record GetCourseEnrollmentsQuery(Guid CourseId) : IQuery<List<EnrollmentDto>>;
