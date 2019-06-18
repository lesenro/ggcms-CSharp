
import {
    styleSave,
    styleDel,
    styleGetById,
    styleGetList,
    templateGetList,
    templateGetContent,
    templateSave,
    templateDel,
    templateUpload,
    templateRename,
    staticGetList,
    staticGetContent,
    staticSave,
    staticDel,
    staticUpload,
    staticRename,
    staticNewDir,
    styleImport
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
            let result = await styleGetList(nParams);
            ctx.commit("setLoading", false);
            return result;
        },
        async save(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await styleSave(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async del(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await styleDel(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async getById(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await styleGetById(params);
            ctx.commit("setLoading", false);
            return result;
        },
        async getTmplList(ctx, params) {
            ctx.commit("setLoading", true);
            let result = await templateGetList(params);
            ctx.commit("setLoading", false);
            return result;
        },
    }
}

export default dictModule;