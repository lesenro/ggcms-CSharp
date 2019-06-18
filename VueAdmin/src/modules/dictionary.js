
import {
    dictSave,
    dictDel,
    dictGetById,
    dictGetList,
    settingsSave
} from '../platformApi';
const dictModule = {
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
            let result = await dictGetList(nParams);
            ctx.commit("setLoading", false);
            return result;
        },
        async save(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await dictSave(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async del(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await dictDel(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async getById(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await dictGetById(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async settingsSave(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await settingsSave(params);
            ctx.commit("setLoading", false);
            return result;
        },
    }
}

export default dictModule;