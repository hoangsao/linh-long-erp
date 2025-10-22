export const hasAnyRole = (userRoles: string[] | undefined, required: string[] | string) => {
  if (!userRoles || userRoles.length === 0) return false;
  const need = Array.isArray(required) ? required : [required];
  return need.some((r) => userRoles.includes(r));
};