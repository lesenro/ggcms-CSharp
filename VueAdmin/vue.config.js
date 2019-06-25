// vue.config.js
module.exports = {
    publicPath: process.env.NODE_ENV === 'production'
    ? '/admin'
    : '/',
    productionSourceMap: false,
    //productionTip : false,
    runtimeCompiler:true,
}