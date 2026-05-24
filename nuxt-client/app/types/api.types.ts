export interface UserDto {
  id: string
  email: string
  firstName: string
  lastName: string
  fullName: string
  isActive: boolean
  createdAt: string
  roles: string[]
  permissions: string[]
}

export interface AuthResponse {
  accessToken: string
  tokenType: string
  expiresIn: number
  user: UserDto
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
