function RefreshCaptcha(txtid,imgid) {
    var code = document.getElementById(txtid);
    code.value = "";
    var img = document.getElementById(imgid);
    img.src = "../CaptchaImage.aspx?Id=" + Math.random();

}