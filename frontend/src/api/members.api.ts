import apiClient from '@/lib/axios'
import type { CreateMemberRequest, CreateMemberResponse, MemberDto, UpdateMemberRequest } from '@/types/member.types'
import type { PagedList } from '@/types/common.types'

export const membersApi = {
  getAll: (params: { page?: number; pageSize?: number; search?: string }) =>
    apiClient.get<PagedList<MemberDto>>('/members', { params }).then(r => r.data),
  getById: (id: string) => apiClient.get<MemberDto>(`/members/${id}`).then(r => r.data),
  create: (payload: CreateMemberRequest) =>
    apiClient.post<CreateMemberResponse>('/members', payload).then(r => r.data),
  update: (id: string, payload: UpdateMemberRequest) =>
    apiClient.put<MemberDto>(`/members/${id}`, payload).then(r => r.data),
  changePassword: (id: string, newPassword: string) =>
    apiClient.patch(`/members/${id}/password`, { newPassword }),
  deactivate: (id: string) => apiClient.patch(`/members/${id}/deactivate`),
}
