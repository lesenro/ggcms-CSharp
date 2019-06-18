
import localForage from "localforage";
import {
    fileUpload,
} from '../platformApi';

const language_key = "language";
//登录公司名
const login_name_key = "login_name_key";
const my_powers_key = "my_powers";
//登录用户
const login_user_key = "login_user";

const appcfg = window["AppConfigs"];
const globalModule = {
    namespaced: true,
    state: {
        open_windows: [],
        active_window: "",
        doc_title: "后台管理",
        themecolor: "20a0ff",
        appcfg: appcfg,
        defaultPageInfo: {
            PageNum: 1,
            PageSize: 10,
            Columns: "",
            QueryString: "",
            WhereParams: null,
            OrderBy: ""
        },
        page_sizes:[10,15,20,30,50,100],
        login_name: "",
        my_powers: [],
        login_user: {}
    },
    getters: {
        screenSize: state => {
            let winWidth = 0;
            if (window.innerWidth) {
                winWidth = window.innerWidth;
            } else if ((document.body) && (document.body.clientWidth)) {
                winWidth = document.body.clientWidth;
            }

            if (winWidth < 768) {
                return "xs"
            }
            if (winWidth >= 768 && winWidth < 992) {
                return "sm"
            }
            if (winWidth >= 992 && winWidth < 1200) {
                return "md"
            }
            if (winWidth >= 1200 && winWidth < 1920) {
                return "lg"
            }
            if (winWidth >= 1920) {
                return "xl"
            }
        },
        lang: state => {
            return localForage.getItem(language_key);
        },
        login_name: state => {
            return localForage.getItem(login_name_key);
        },
    },
    mutations: {
        getLoginUser(state) {
            localForage.getItem(login_user_key).then(x => {
                state.login_user = x;
            });
        },
        setOpenWindows(state, wins) {
            state.open_windows = wins;
        },
        setActiveWindow(state, path) {
            state.active_window = path;
        },
        //更新主题颜色
        setThemeColor(state, curcolor) {
            state.themecolor = curcolor;
        },
        setLoginName(state, name) {
            state.login_name = name;
            localForage.setItem(login_name_key, name);
        },
        setMyPowers(state, powers) {
            state.my_powers = powers;
            localForage.setItem(my_powers_key, powers);
        },
        setLoginUser(state, info) {
            state.login_user = info;
            localForage.setItem(login_user_key, info);
        },
        setLanguage(state, lang) {
            localForage.setItem(language_key, lang);
        },
        setDocTitle(state, title) {
            if (title) {
                document.title = title + "-" + state.doc_title;
            }
        },

    },
    actions: {
        submit({ commit }) {
            commit('submit')
        },
        setMyPowers({ commit }, powers) {
            commit("setMyPowers", powers)
        },
        async  getMyPowers({ commit }) {
            let powers = await localForage.getItem(my_powers_key);
            return powers;
        },
        async  getLoginUser({ commit }) {
            let result = await localForage.getItem(login_user_key);
            return result;
        },
        async fileUpload(ctx, params) {
            let result = await fileUpload(params);
            return result;
        },
    }
}

export default globalModule;