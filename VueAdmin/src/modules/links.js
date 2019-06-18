
import {
    linksSave,
    linksDel,
    linksGetById,
    linksGetList,
} from '../platformApi';
const linkModule = {
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
            let result = await linksGetList(nParams);
            ctx.commit("setLoading", false);
            return result;
        },
        async save(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await linksSave(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async del(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await linksDel(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async getById(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await linksGetById(params);
            ctx.commit("setLoading", false);
            return result;
        },
    }
}

export default linkModule;