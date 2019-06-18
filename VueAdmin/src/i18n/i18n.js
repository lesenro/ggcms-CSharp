//i18n.js
//参考:https://segmentfault.com/a/1190000012779120
import Vue from 'vue'
import locale from 'element-ui/lib/locale'
import VueI18n from 'vue-i18n'
import en from './en'
import cn from './cn'
import store from '../modules/app_store'

const messages = {
    en, cn,
};
Vue.use(VueI18n)

const i18n = new VueI18n({
    locale: "cn",
    messages
})
locale.i18n((key, value) => i18n.t(key, value)) //重点：为了实现element插件的多语言切换
store.getters["global/lang"].then(x => {
    i18n.locale = x||"cn";
});
export default i18n;