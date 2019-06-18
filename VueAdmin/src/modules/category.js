
import {
    categorySave,
    categorySortSave,
    categoryDel,
    categoryGetById,
    categoryGetList
} from '../platformApi';
const categoryModule = {
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
            let result = await categoryGetList(nParams);
            ctx.commit("setLoading", false);
            return result;
        },
        async save(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await categorySave(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async sortSave(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await categorySortSave(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async del(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await categoryDel(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async getById(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await categoryGetById(params);
            ctx.commit("setLoading", false);
            return result;
        },
    }
}

export default categoryModule;