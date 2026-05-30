export interface MemberDto {
  id: string
  email: string
  firstName: string
  lastName: string
  fullName: string
  isActive: boolean
  mustChangePassword: boolean
  createdAt: string
  lastLoginAt: string | null
}

export interface MemberAuthResponse {
  accessToken: string
  tokenType: string
  expiresIn: number
  member: MemberDto
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  email: string
  password: string
  firstName: string
  lastName: string
}

export interface ApiError {
  code: string
  message: string
  details?: Record<string, string[]>
}
