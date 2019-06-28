
import {
    modulesSave,
    modulesDel,
    modulesGetById,
    modulesGetList,
} from '../platformApi';
const dataModel = {
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
            let result = await modulesGetList(nParams);
            ctx.commit("setLoading", false);
            return result;
        },
        async save(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await modulesSave(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async del(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await modulesDel(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async getById(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await modulesGetById(params);
            ctx.commit("setLoading", false);
            return result;
        },

    }
}

export default dataModel;