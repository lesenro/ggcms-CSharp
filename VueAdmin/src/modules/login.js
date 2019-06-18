import {
    login,
    powerGetList,
    logout,
    getCodeImgUrl
} from '../platformApi';
import globalModule from './global'

const loginModule = {
    namespaced: true,
    state: {
        codeImgUrl: "",
    },
    mutations: {
        getCodeImg(state) {
            state.codeImgUrl = getCodeImgUrl();
        }
    },
    actions: {
        async login(ctx, params) {
            let result = login(params)
            return result;
        },
        async logout(ctx) {
            let result = logout();
            return result;
        },
        async getPowers(ctx) {
            let result = await powerGetList();
            return result;
        },
    }
}

export default loginModule;