import request from './utils/request';
import { AppTools } from './utils/appTools';
import { stringify } from 'qs';
import globalStore from './modules/global';
const svrUrl = globalStore.state.appcfg.ServerUrl || "";
const apiUrl = svrUrl + '/' + (globalStore.state.appcfg.ApiPath || "");

// 登录
// username,password
export async function login(params) {
    params.imgcode = params.captcha.toLowerCase();
    params.password = AppTools.Md5(params.password);
    params.password = AppTools.Md5(params.password + params.captcha);
    let url = apiUrl;
    const data = Object.assign({}, params);
    return request(`${url}/Login/PostLogin`, {
        method: 'POST',
        body: { ...data },
    });
}


// 登出
export async function logout() {
    return request(`${apiUrl}/Login/GetLogout`, {
        method: 'Get',
    });
}
// 获取登录用户
export async function getLoginInfo() {
    return request(`${apiUrl}/Login/GetLoginInfo`, {
        method: 'Get',
    });
}


// 获取图型验证码
// data:image/png;base64,
export function getCodeImgUrl() {
    const rnd = (new Date()).getTime().toString();
    const url = svrUrl + `/getCaptcha?${rnd}`;
    return url;

}

// 获取图型验证码
// data:image/png;base64,
export function fileUpload(params) {
    const url = apiUrl + `/Common/fileUpload`;
    let formData = new FormData();
    formData.append('file', params.file);
    formData.append('type', params.type);
    return request(url, {
        method: 'POST',
        body: formData,
    });
}


//保存分类信息
export async function categorySave(params) {
    let action = params.Id > 0 ? "Edit" : "Add";
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsCategories/${action}`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}
//保存分类排序信息
export async function categorySortSave(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsCategories/CategorySortSave`, {
        method: 'POST',
        body: params,
    });
}

//删除分类信息
export async function categoryDel(id) {
    return request(`${apiUrl}/GgcmsCategories/Delete/${id}`, {
        method: 'Get'
    });
}

//获取分类详情
export async function categoryGetById(id) {
    return request(`${apiUrl}/GgcmsCategories/GetInfo/${id}`, {
        method: 'Get'
    });
}

//获取网站导航菜单
export async function categoryGetList(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsCategories/GetList?query=${query}`, {
        method: 'Get'
    });
}


//获取权限菜单
export async function powerGetList() {
    let query = JSON.stringify({
        PageNum: 1,
    });
    return request(`${apiUrl}/GgcmsPowers/GetList?query=${query}`, {
        method: 'Get'
    });
}


//保存文章信息
export async function articleSave(params) {
    let action = params.Id > 0 ? "Edit" : "Add";
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsArticles/${action}`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除文章信息
export async function articleDel(ids) {
    return request(`${apiUrl}/GgcmsArticles/MultDelete/?ids=${ids.join(",")}`, {
        method: 'Get'
    });
}

//获取分类详情
export async function articleGetById(id) {
    return request(`${apiUrl}/GgcmsArticles/GetInfo/${id}`, {
        method: 'Get'
    });
}

//获取网站导航菜单
export async function articleGetList(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsArticles/GetList?query=${query}`, {
        method: 'Get'
    });
}

//保存系统字典
export async function dictSave(params) {
    let action = params.Id > 0 ? "Edit" : "Add";
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsDictionaries/${action}`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}


//删除系统字典
export async function dictDel(params) {
    return request(`${apiUrl}/GgcmsDictionaries/MultDelete`, {
        method: 'POST',
        body: params,
    });
}


//获取系统字典
export async function dictGetById(id) {
    return request(`${apiUrl}/GgcmsDictionaries/GetInfo/${id}`, {
        method: 'Get'
    });
}

//获取系统字典列表
export async function dictGetList(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsDictionaries/GetList?query=${query}`, {
        method: 'Get'
    });
}


//保存设置
export async function settingsSave(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsDictionaries/SettingsSave`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}


//保存风格模板
export async function styleSave(params) {
    let action = params.Id > 0 ? "Edit" : "Add";
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/${action}`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除风格模板
export async function styleDel(ids) {
    return request(`${apiUrl}/GgcmsStyles/MultDelete?ids=${ids}`, {
        method: 'Get'
    });
}


//获取风格详情
export async function styleGetById(id) {
    return request(`${apiUrl}/GgcmsStyles/GetInfo/${id}`, {
        method: 'Get'
    });
}


//获取风格列表
export async function styleGetList(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsStyles/GetList?query=${query}`, {
        method: 'Get'
    });
}

//获取模板文件列表
export async function templateGetList(id) {
    return request(`${apiUrl}/GgcmsStyles/GetTemplateList/${id}`, {
        method: 'Get'
    });
}

//获取模板文件内容
export async function templateGetContent(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsStyles/GetTemplateInfo?${query}`, {
        method: 'Get'
    });
}

//获取模板文件内容
export async function templateSave(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/TemplateFileSave`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除模板文件
export async function templateDel(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/TemplateFileDelete`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除模板文件
export async function templateUpload(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/TemplateFileUpload`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//模板重命名
export async function templateRename(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/TemplateFileReName`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}


//=====================
//获取静态文件列表
export async function staticGetList(id) {
    return request(`${apiUrl}/GgcmsStyles/GetStaticFile/${id}`, {
        method: 'Get'
    });
}

//获取模板文件内容
export async function staticGetContent(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsStyles/GetStaticFile?${query}`, {
        method: 'Get'
    });
}

//获取模板文件内容
export async function staticSave(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/GetStaticFile`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除模板文件
export async function staticDel(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/GetStaticFile`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除模板文件
export async function staticUpload(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/GetStaticFile`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//模板重命名
export async function staticRename(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/StaticFileReName`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}
//新建文件夹
export async function staticNewDir(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/StaticFileNewDir`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}


//导入风格模板
export async function styleImport(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsStyles/StyleImport`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}
//密码修改
export async function modifyPassword(params) {
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsMembers/ModifyPassword`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}
//清理缓存
export async function clearCache(params) {
    return request(`${apiUrl}/Common/ClearCache`, {
        method: 'Get',
    });
}


//应用重置
export async function appRestart(params) {
    return request(`${apiUrl}/Common/AppRestart`, {
        method: 'Get',
    });
}

//保存友情链接
export async function linksSave(params) {
    let action = params.Id > 0 ? "Edit" : "Add";
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsFriendLinks/${action}`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除友情链接
export async function linksDel(params) {
    return request(`${apiUrl}/GgcmsFriendLinks/MultDelete`, {
        method: 'POST',
        body: params,
    });
}

//获取友情链接详情
export async function linksGetById(id) {
    return request(`${apiUrl}/GgcmsFriendLinks/GetInfo/${id}`, {
        method: 'Get'
    });
}


//获取网站导航菜单
export async function linksGetList(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsFriendLinks/GetList?query=${query}`, {
        method: 'Get'
    });
}
//======================================
//保存广告
export async function advertsSave(params) {
    let action = params.Id > 0 ? "Edit" : "Add";
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsAdverts/${action}`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除广告
export async function advertsDel(params) {
    return request(`${apiUrl}/GgcmsAdverts/MultDelete`, {
        method: 'POST',
        body: params,
    });
}

//获取广告详情
export async function advertsGetById(id) {
    return request(`${apiUrl}/GgcmsAdverts/GetInfo/${id}`, {
        method: 'Get'
    });
}


//获取广告列表
export async function advertsGetList(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsAdverts/GetList?query=${query}`, {
        method: 'Get'
    });
}
//======================================
//保存数据模型
export async function modulesSave(params) {
    let action = params.Id > 0 ? "Edit" : "Add";
    const data = Object.assign({}, params);
    return request(`${apiUrl}/GgcmsModules/${action}`, {
        method: 'POST',
        body: {
            ...data,
        },
    });
}

//删除数据模型
export async function modulesDel(id) {
    return request(`${apiUrl}/GgcmsModules/Delete/${id}`, {
        method: 'Get'
    });
}

//获取数据模型详情
export async function modulesGetById(id) {
    return request(`${apiUrl}/GgcmsModules/GetInfo/${id}`, {
        method: 'Get'
    });
}


//获取数据模型列表
export async function modulesGetList(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsModules/GetList?query=${query}`, {
        method: 'Get'
    });
}
//获取数据模型对应文章内容
export async function modulesGetValue(params) {
    const query = JSON.stringify(params);
    return request(`${apiUrl}/GgcmsModules/GetGgcmsModuleValue?${query}`, {
        method: 'Get'
    });
}

