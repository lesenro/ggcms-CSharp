import { routerRedux } from 'dva/router';
import { adminLogin, logout,GetLoginUser } from '../services/platformApi';

export default {
  namespace: 'login',

  state: {
    status: undefined,
  },

  effects: {
    *adminLogin({ payload, callback }, { call, put }) {
      yield put({
        type: 'changeSubmitting',
        payload: true,
      });
      const response = yield call(adminLogin, payload);
      yield put({
        type: 'changeSubmitting',
        payload: false,
      });
      if (callback) callback(response);

      if (response.Code === 0) {
        yield put(routerRedux.push('/'));
      }
    },

    *logout({ callback }, { call, put }) {
      yield put({
        type: 'changeSubmitting',
        payload: true,
      });
      const response = yield call(logout);
      yield put({
        type: 'changeSubmitting',
        payload: false,
      });
      if (callback) callback(response);
      if (response.Code === 0) {
        yield put(routerRedux.push('/user/login'));
      }
    },
    *getLoginUser({ callback }, { call, put }) {
      yield put({
        type: 'changeSubmitting',
        payload: true,
      });
      const response = yield call(GetLoginUser);
      yield put({
        type: 'changeSubmitting',
        payload: false,
      });
      if (callback) callback(response);
    },
    
  },

  reducers: {
    changeLoginStatus(state, { payload }) {
      return {
        ...state,
        status: payload.status,
        type: payload.type,
      };
    },
    changeSubmitting(state, { payload }) {
      return {
        ...state,
        submitting: payload,
      };
    },
  },
};
