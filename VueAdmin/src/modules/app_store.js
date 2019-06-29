
import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex);

import loginModule from './login'
import globalModule from './global'
import dictModule from './dictionary'
import stylesModule from './styles'
import advModule from './adverts'
import linkModule from './links'
import categoryModule from './category'
import articleModule from './article'
import userModule from './users'
import dataModel from './data_model'
import taskModel from './tasks'
const store = new Vuex.Store({
    modules: {
        global: globalModule,
        login: loginModule,
        dict: dictModule,
        styles: stylesModule,
        advert: advModule,
        link: linkModule,
        category: categoryModule,
        article: articleModule,
        user: userModule,
        extMod: dataModel,
        task: taskModel,
    }
});
export default store;