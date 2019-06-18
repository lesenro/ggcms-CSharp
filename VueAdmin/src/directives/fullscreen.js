import Vue from 'vue'
const fscr_name = "full-screen";
export default Vue.directive(fscr_name, {
    update: function (el, binding) {
        let classList = el.className.split(" ");
        if (binding.value) {
            classList.push(fscr_name);
            el.className = classList.join(" ");
        } else {
            el.className = classList.filter(x => x != fscr_name).join(" ");
        }
        FullScreen(el);
    }
});
function FullScreen(el) {
    var isFullscreen = document.fullScreen || document.mozFullScreen || document.webkitIsFullScreen;
    if (!isFullscreen) {//进入全屏,多重短路表达式
        (el.requestFullscreen && el.requestFullscreen()) ||
            (el.mozRequestFullScreen && el.mozRequestFullScreen()) ||
            (el.webkitRequestFullscreen && el.webkitRequestFullscreen()) || (el.msRequestFullscreen && el.msRequestFullscreen());

    } else {	//退出全屏,三目运算符
        document.exitFullscreen ? document.exitFullscreen() :
            document.mozCancelFullScreen ? document.mozCancelFullScreen() :
                document.webkitExitFullscreen ? document.webkitExitFullscreen() : '';
    }
}