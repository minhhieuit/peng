import api from '@/lib/axios'
import type { CourseDto, EnrollmentDto, CreateCourseRequest, UpdateCourseRequest } from '@/types/courses.types'
import type { PagedList } from '@/types/common.types'

export const coursesApi = {
  getAll: (page = 1, pageSize = 20) =>
    api.get<PagedList<CourseDto>>('/manage/courses', { params: { page, pageSize } }).then(r => r.data),
  getById: (id: string) =>
    api.get<CourseDto>(`/manage/courses/${id}`).then(r => r.data),
  create: (data: CreateCourseRequest) =>
    api.post<CourseDto>('/manage/courses', data).then(r => r.data),
  update: (id: string, data: UpdateCourseRequest) =>
    api.put<CourseDto>(`/manage/courses/${id}`, data).then(r => r.data),
  remove: (id: string) =>
    api.delete(`/manage/courses/${id}`),
  togglePublish: (id: string) =>
    api.patch<CourseDto>(`/manage/courses/${id}/publish`).then(r => r.data),
  getEnrollments: (id: string) =>
    api.get<EnrollmentDto[]>(`/manage/courses/${id}/enrollments`).then(r => r.data),
}
