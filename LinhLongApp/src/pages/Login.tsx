import React, { useState } from 'react';
import { Button, Card, Form, Input, Typography, Alert } from 'antd';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../store/authStore';
import { extractApiMessage } from '../shared/api/errors';

const { Title, Text } = Typography;

const Login: React.FC = () => {
  const login = useAuthStore((s) => s.login);
  const navigate = useNavigate();
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const onFinish = async (values: { username: string; password: string }) => {
    setError(null);
    setSubmitting(true);
    try {
      await login(values.username, values.password);
      navigate('/', { replace: true });
    } catch (e: unknown) {
      setError(extractApiMessage(e, 'Login failed. Please check your credentials.'));
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', minHeight: '100vh', padding: 16 }}>
      <Card style={{ width: 420 }}>
        <Title level={3} style={{ marginBottom: 8 }}>Sign in</Title>
        <Text type="secondary">Use your LinhLong account</Text>
        {error && (
          <Alert style={{ marginTop: 16 }} type="error" message={error} showIcon />
        )}
        <Form layout="vertical" style={{ marginTop: 16 }} onFinish={onFinish}>
          <Form.Item label="Username" name="username" rules={[{ required: true, message: 'Please input your username' }]}>
            <Input autoFocus autoComplete="username" />
          </Form.Item>
          <Form.Item label="Password" name="password" rules={[{ required: true, message: 'Please input your password' }, { min: 12, message: 'Password must be at least 12 characters (per policy)' }]}>
            <Input.Password autoComplete="current-password" />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" block loading={submitting}>Sign in</Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
};

export default Login;