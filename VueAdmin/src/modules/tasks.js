
import {
    tasksSave,
    tasksDel,
    tasksGetById,
    tasksGetList,
    runNow
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
            let result = await tasksGetList(nParams);
            ctx.commit("setLoading", false);
            return result;
        },
        async save(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await tasksSave(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async del(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await tasksDel(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async getById(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await tasksGetById(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async runNow(ctx, id) {
            ctx.commit("setLoading", true);
            let result = await runNow(id);
            ctx.commit("setLoading", false);
            return result;
        },
    }
}

export default linkModule;