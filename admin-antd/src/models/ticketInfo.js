import { getTicketInfo, deleteAllTicket } from '../services/platformApi';
import { AppTools } from '../utils/utils';
import { routerRedux } from 'dva/router';

export default {
  namespace: 'ticketInfo',

  state: {
    data: {},
    loading: true,
  },

  effects: {
    *getTicketInfo({ payload }, { call, put }) {
      yield put({
        type: 'changeLoading',
        payload: true,
      });
      const response = yield call(getTicketInfo, payload);
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
    *deleteAllTicket({ payload, callback }, { call, put }) {
      yield put({
        type: 'changeLoading',
        payload: true,
      });
      const response = yield call(deleteAllTicket);

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
