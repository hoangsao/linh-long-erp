import React from 'react';
import { Tag, Typography } from 'antd';
import { useAuthStore } from '../../../store/authStore';

const { Text } = Typography;

export const UserBadge: React.FC = () => {
  const user = useAuthStore((s) => s.user);
  if (!user) return null;
  return (
    <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
      <Text style={{ color: 'white' }}>{user.userName}</Text>
      {user.roles.map((r) => (
        <Tag key={r} color={r === 'Editor' ? 'green' : 'blue'}>{r}</Tag>
      ))}
    </div>
  );
};