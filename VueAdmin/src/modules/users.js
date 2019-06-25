
import {
    userSave,
    userDel,
    userGetById,
    userGetList,
    modifyPassword
} from '../platformApi';
const userModule = {
    namespaced: true,
    state: {
        loading: false,
    },
    mutations: {
        setLoading(state, val) {
            state.loading = val;
        },
    },
    actions: {
        async getList(ctx, params) {
            ctx.commit("setLoading", true);
            let nParams = Object.assign({}, params);
            let result = await userGetList(nParams);
            ctx.commit("setLoading", false);
            return result;
        },
        async save(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await userSave(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async del(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await userDel(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async getById(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await userGetById(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async modifyPassword(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await modifyPassword(params);
            ctx.commit("setLoading", false);
            return result;
        },
    }
}

export default userModule;