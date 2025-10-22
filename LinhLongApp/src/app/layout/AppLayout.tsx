import React, { useEffect, useState } from 'react';
import { matchRoutes, useLocation, useNavigate } from 'react-router-dom';
import { Layout, Menu, type MenuProps } from 'antd';
import { HomeOutlined, LogoutOutlined, UserOutlined } from '@ant-design/icons';
import { APP_ROUTES } from '../../shared/constants/routes';
import { useAuthStore } from '../../store/authStore';
import LogoUrl from '../../assets/logo.png';
import './AppLayout.scss';

const { Header, Content, Footer } = Layout;
type MenuItem = Required<MenuProps>['items'][number];

export const AppLayout: React.FC<React.PropsWithChildren> = ({ children }) => {
  const { logout, user } = useAuthStore();
  const navigate = useNavigate()
  const location = useLocation()
  const [currentKey, setCurrentKey] = useState<string>(APP_ROUTES.HOME.KEY)

  const handleLogout = async () => {
    await logout()
    navigate(APP_ROUTES.LOGIN.ROUTE, { replace: true })
  }

  const getRoute = (key: string) => {
    const route = Object.values(APP_ROUTES).find((route) => route.KEY === key)
    return route
  }

  const onLeftNavClick: MenuProps['onClick'] = (e) => {
    const route = getRoute(e.key)
    if (route) {
      navigate(route.ROUTE)
    }
  }

  const onRightNavClick: MenuProps['onClick'] = (e) => {
    if (e.key === 'logout') {
      handleLogout()
    }
  }

  const renderLeftNav = () => {
    const leftNavItems: MenuItem[] = [
      {
        key: APP_ROUTES.HOME.KEY,
        label: APP_ROUTES.HOME.TITLE,
        icon: <HomeOutlined />,
      }
    ]

    return <Menu className='main-menu left-nav' theme="dark" onClick={onLeftNavClick} selectedKeys={[currentKey]} mode="horizontal" items={leftNavItems} />
  }

  const renderRightNav = () => {
    if (!user) {
      return
    }

    const rightNavItems: MenuItem[] = [
      {
        key: 'profile-group',
        icon: <UserOutlined />,
        label: user.userName,
        children: [
          {
            key: 'logout',
            label: 'Logout',
            icon: <LogoutOutlined />
          },
        ],
      }
    ]

    return <Menu className='user-menu right-nav' theme="dark" onClick={onRightNavClick} mode="horizontal" items={rightNavItems} selectable={false} />
  }

  useEffect(() => {
    const routes = Object.values(APP_ROUTES).map((route) => {
      return {
        ...route,
        path: route.ROUTE,
      }
    });

    const matches = matchRoutes(routes, location.pathname);
    if (matches) {
      setCurrentKey(matches[0].route.KEY)
    }
    else {
      setCurrentKey('')
    }
  }, [location.pathname])

  return (
    <Layout>
      <Header className="header">
        <div className="logo"><img src={LogoUrl} alt='' /></div>
        {renderLeftNav()}
        {renderRightNav()}
      </Header>
      <Content className="content">
        {children}
      </Content>
      <Footer className="footer">Linh Long ©2025 Created by Sao Dao</Footer>
    </Layout>
  );
};