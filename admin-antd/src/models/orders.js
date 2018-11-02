import { getTicketOrder, getDetailByOrderId, getOrderTotal } from '../services/platformApi';
import { AppTools } from '../utils/utils';
import { routerRedux } from 'dva/router';

export default {
  namespace: 'orders',

  state: {
    data: {},
    loading: true,
  },

  effects: {
    *getTicketOrder({ payload }, { call, put }) {
      yield put({
        type: 'changeLoading',
        payload: true,
      });
      const response = yield call(getTicketOrder, payload);
      yield put({
        type: 'save',
        payload: {
          pageData: response,
        },
      });
      yield put({
        type: 'changeLoading',
        payload: false,
      });
    },
    *getDetailByOrderId({ payload, callback }, { call, put }) {
      yield put({
        type: 'changeLoading',
        payload: true,
      });
      const response = yield call(getDetailByOrderId, payload);

      yield put({
        type: 'changeLoading',
        payload: false,
      });
      if (callback) callback(response);
    },
  },

  reducers: {
    save(state, action) {
      return {
        ...state,
        data: action.payload,
      };
    },
    saveCurrentUser(state, action) {
      return {
        ...state,
      };
    },
    changeLoading(state, action) {
      return {
        ...state,
        loading: action.payload,
      };
    },
  },
};
