import { getRefundInfo } from '../services/platformApi';
import { AppTools } from '../utils/utils';
import { routerRedux } from 'dva/router';

export default {
  namespace: 'refund',

  state: {
    data: {},
    loading: true,
  },

  effects: {
    *getRefundInfo({ payload }, { call, put }) {
      yield put({
        type: 'changeLoading',
        payload: true,
      });
      const response = yield call(getRefundInfo, payload);
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
