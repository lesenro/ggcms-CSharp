import md5 from 'blueimp-md5';
import request from '../utils/request';
import { AppTools } from '../utils/utils';

const apiUrl = window.AppConfigs.ApiUrl || '';
const apiPath = window.AppConfigs.ApiPath || '';

const basePageParams = {
  currentPage: 1,
  pageSize: 10,
};


export function CaptchaUrl(){
  const timestamp=(new Date()).getTime().toString();
  return `${apiUrl}/getCaptcha?${timestamp}`;
}
// 登录
// username,password
export async function adminLogin(params) {
  const data = Object.assign({}, params);
  data.password=md5(data.password);
  data.password=md5(data.password+data.captcha);
  return request(`${apiUrl+apiPath}/Login/PostLogin`, {
    method: 'POST',
    body: {
      ...data,
    },
  }).then(resp => {
    if (resp.access_token) {
      AppTools.AccessToken(resp.access_token);
    }
    return resp;
  });
}
// 登出
export async function logout() {
  return request(`${apiUrl+apiPath}/Login/GetLogout`, {
    method: 'GET',
  }).then(resp => {
    if (resp.code === 0) {
      AppTools.RemoveToken();
    }
    return resp;
  });
}
// 客户信息
// name,tel,currentPage,pageSize
export async function GetLoginUser() {
  const url = `${apiUrl+apiPath}/Login/GetLoginUser`;
  return request( url, {
    method: 'GET',
  });
}



// 客户信息
// name,tel,currentPage,pageSize
export async function getCustomer(params) {
  const data = Object.assign({}, basePageParams, params);
  const url = '/customer/getCustomer';
  return request(apiUrl + url, {
    method: 'POST',
    body: {
      ...data,
    },
  });
}

// 退票信息
// linkName,linkPhone,productCode,currentPage,pageSize
export async function getRefundInfo(params) {
  const data = Object.assign({}, basePageParams, params);
  const url = '/refund/getRefundInfo';
  return request(apiUrl + url, {
    method: 'POST',
    body: {
      ...data,
    },
  });
}

// 票务信息
// productName,currentPage,pageSize
export async function getTicketInfo(params) {
  const data = Object.assign({}, basePageParams, params);
  const url = '/ticketInfo/getTicketInfo';
  return request(apiUrl + url, {
    method: 'POST',
    body: {
      ...data,
    },
  });
}

// 删除所有票务信息
export async function deleteAllTicket() {
  const url = '/ticketInfo/deleteAll';
  return request(apiUrl + url, {
    method: 'POST',
    body: {},
  });
}

// 订单查询
// name,tel,currentPage,pageSize
export async function getTicketOrder(params) {
  const data = Object.assign({}, basePageParams, params);
  const url = '/order/getTicketOrder';
  return request(apiUrl + url, {
    method: 'POST',
    body: {
      ...data,
    },
  });
}
// 查询订单明细
// orderId
export async function getDetailByOrderId(params) {
  const url = '/detail/getDetailByOrderId';
  return request(apiUrl + url, {
    method: 'POST',
    body: {
      ...params,
    },
  });
}

// 订单信息统计
// startTime,endTime
export async function getDetailTotal(params) {
  const url = '/detail/getTotal';
  return request(apiUrl + url, {
    method: 'POST',
    body: {
      ...params,
    },
  });
}

