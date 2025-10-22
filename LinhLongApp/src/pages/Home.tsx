import React from 'react';
import { Card, Tag, Typography, Space } from 'antd';
import { useAuthStore } from '../store/authStore';


const { Title, Text } = Typography;

const Home: React.FC = () => {
  const user = useAuthStore((s) => s.user);

  return (
    <div className="app-padding">
      <Card>
        <Space direction="vertical" size="middle" style={{ width: '100%' }}>
          <Title level={4} style={{ margin: 0 }}>Welcome</Title>
          <Text>Signed in as <b>{user?.userName}</b></Text>
          <div>
            <Text type="secondary">Roles:</Text>{' '}
            {user?.roles?.length ? user.roles.map((r) => (
              <Tag key={r} color={r === 'Editor' ? 'green' : 'blue'}>{r}</Tag>
            )) : <Tag>None</Tag>}
          </div>
        </Space>
      </Card>
    </div>
  );
};

export default Home;