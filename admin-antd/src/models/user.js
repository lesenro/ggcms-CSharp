import { getCustomer } from '../services/platformApi';

export default {
  namespace: 'user',

  state: {
    data: {},
    loading: true,
    currentUser: {},
  },

  effects: {
    *getCustomer({ payload }, { call, put }) {
      yield put({
        type: 'changeLoading',
        payload: true,
      });
      const response = yield call(getCustomer, payload);
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
        currentUser: action.payload,
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
