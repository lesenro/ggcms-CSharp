/* eslint-disable no-unused-vars */
import Vue from 'vue';
import App from './App.vue';
import VueRouter from 'vue-router';
import store from './modules/app_store';
import router from './routers';
import ElementUI from 'element-ui';
import './element-variables.scss'
import 'element-ui/lib/theme-chalk/display.css';
import i18n from './i18n/i18n';
import FormGenerator from "./components/FormGenerator/FormGenerator";
import moment from 'moment';
import 'linqjs';

import VueCodemirror from 'vue-codemirror';
import 'codemirror/lib/codemirror.css';
import 'codemirror/theme/monokai.css';
import 'codemirror/mode/xml/xml.js';

Vue.use(VueCodemirror, {
  options: {
    theme: 'monokai',
    tabSize: 4,
    styleActiveLine: true,
    lineNumbers: true,
    autoCloseTags: true,
    line: true,
  },

});

Vue.filter('dateformat', function (dataStr, pattern = 'YYYY-MM-DD HH:mm:ss') {
  return moment(dataStr).format(pattern)
});

Vue.use(ElementUI, {
  i18n: (key, value) => i18n.t(key, value)
});
// globally (in your main .js file)
Vue.component('form-generator', FormGenerator);

Vue.use(VueRouter);
let myVueApp = new Vue({
  router,
  store,
  i18n,
  render: h => h(App),
}).$mount('#app');
