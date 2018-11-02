import { isUrl } from '../utils/utils';

const menuData = [
  {
    name: '管理首页',
    icon: 'home',
    path: 'home',
  },
  {
    name: '客户信息',
    icon: 'team',
    path: 'customer',
  },
  {
    name: '票务信息',
    icon: 'idcard',
    path: 'ticket',
  },
  {
    name: '订单管理',
    icon: 'profile',
    path: 'orders',
  },
  {
    name: '退票查询',
    icon: 'exception',
    path: 'refund',
  },
  {
    name: '订单统计',
    icon: 'bar-chart',
    path: 'count',
  },
];

function formatter(data, parentPath = '/', parentAuthority) {
  return data.map(item => {
    let { path } = item;
    if (!isUrl(path)) {
      path = parentPath + item.path;
    }
    const result = {
      ...item,
      path,
      authority: item.authority || parentAuthority,
    };
    if (item.children) {
      result.children = formatter(item.children, `${parentPath}${item.path}/`, item.authority);
    }
    return result;
  });
}

export const getMenuData = () => formatter(menuData);
