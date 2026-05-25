export interface CourseDto {
  id: string
  title: string
  description: string
  thumbnailUrl: string | null
  price: number
  isPublished: boolean
  instructorId: string
  enrollmentCount: number
  createdAt: string
}

export interface EnrollmentDto {
  id: string
  courseId: string
  courseTitle: string
  userId: string
  userName: string | null
  userEmail: string | null
  status: 'Active' | 'Cancelled'
  enrolledAt: string
}

export interface CourseStatsDto {
  total: number
  published: number
  drafts: number
  totalEnrollments: number
}

export type CourseStatusFilter = 'All' | 'Published' | 'Draft'

export interface CourseListParams {
  page?: number
  pageSize?: number
  search?: string
  status?: CourseStatusFilter
}

export interface CreateCourseRequest {
  title: string
  description: string
  price: number
  thumbnailUrl?: string
}

export interface UpdateCourseRequest {
  title: string
  description: string
  price: number
  thumbnailUrl?: string
}
