import moment from 'moment';
import CryptoJS from 'crypto-js';
const defaultSecretKey = "12345678";
export class AppTools {
    //   static AccessToken(token) {
    //     if (token) {
    //       Cookies.set(CookieNames.access_token, token, { path: '/' });
    //     } else {
    //       return Cookies.get(CookieNames.access_token);
    //     }
    //   }
    //   static RemoveToken() {
    //     Cookies.remove(CookieNames.access_token);
    //   }
    static Base64(input) {
        let keyStr = "ABCDEFGHIJKLMNOP" + "QRSTUVWXYZabcdef" + "ghijklmnopqrstuv"
            + "wxyz0123456789+/" + "=";
        let output = "";
        let chr1, chr2, chr3 = "";
        let enc1, enc2, enc3, enc4 = "";
        let i = 0;
        do {
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);
            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;
            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }
            output = output + keyStr.charAt(enc1) + keyStr.charAt(enc2)
                + keyStr.charAt(enc3) + keyStr.charAt(enc4);
            chr1 = chr2 = chr3 = "";
            enc1 = enc2 = enc3 = enc4 = "";
        } while (i < input.length);

        return output;
    }
    static Base64Stringify(val) {
        let wordArray = CryptoJS.enc.Utf8.parse(val);
        return CryptoJS.enc.Base64.stringify(wordArray);
    }
    static Base64Parse(val) {
        let parsedWordArray = CryptoJS.enc.Base64.parse(val);
        return parsedWordArray.toString(CryptoJS.enc.Utf8);
    }
    static DesEncrypt(val, skey = defaultSecretKey) {
        return CryptoJS.TripleDES.encrypt(val, skey).toString();
    }
    static DesDecrypt(val, skey = defaultSecretKey) {
        return CryptoJS.TripleDES.decrypt(val, skey).toString(CryptoJS.enc.Utf8);
    }
    static Md5(val) {
        return CryptoJS.MD5(val).toString();
    }
    static getPageInfo(pagination) {
        return {
            pageSize: pagination.pageSize,
            defaultCurrent: pagination.pageNum,
            current: pagination.pageNum,
            total: pagination.total,
            showSizeChanger: false,
            showQuickJumper: false,
        }
    }
    static arrayToObject(arrList, nameStr = "name", valueStr = "value") {
        const obj = {};
        arrList.forEach(item => {
            obj[item[nameStr]] = item[valueStr];
        });
        return obj;
    }
    static timeStamp(time = new Date()) {
        return time.getTime();
    }
    static downfile(url) {
        let link = document.createElement("a");
        link.href=url;
        link.target="_blank";
        link.click();
    }
}