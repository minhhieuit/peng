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

export interface CreateMemberRequest {
  email: string
  firstName: string
  lastName: string
}

export interface CreateMemberResponse {
  id: string
  email: string
  fullName: string
  temporaryPassword: string
}

export interface UpdateMemberRequest {
  firstName: string
  lastName: string
}
