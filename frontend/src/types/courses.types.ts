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
  status: 'Active' | 'Cancelled'
  enrolledAt: string
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
