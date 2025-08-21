export interface UserDto {
  id: number;
  email: string;
  fullName: string;
  isActive: boolean;
  roleId: number;
  role: string;
  createdAt: string;
  updatedAt?: string | null;
}
