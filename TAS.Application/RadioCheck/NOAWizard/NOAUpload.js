function UploadComplete(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);

        if (tokens != null) {
            $get("lblUploadMsg").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
            $get("fuAttachment_ctl02").value = "";
            $get("pnlFile").style.display = "";
            $get("fuAttachment_ctl02").style.display = "none";
            $get("LoadingImg").style.display = "none";
        }
        else {
            $get("lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
            $get("fuAttachment_ctl02").style.backgroundColor = "Red";
            $get("LoadingImg").style.display = "none";
        }

    }
    else {
        $get("lblUploadMsg").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("fuAttachment_ctl02").style.backgroundColor = "Red";
        $get("LoadingImg").style.display = "none";
        return;
    }
}

function UploadError(sender, args) {
    $get("lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("fuAttachment_ctl02").style.backgroundColor = "Red";
    $get("LoadingImg").style.display = "none";
}

function StartUpload(sender, args) {
    $get("LoadingImg").style.display = "";
    $get("lblUploadMsg").innerText = "";
    //$get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.display = "none";
    $get("fuAttachment_ctl02").style.backgroundColor = "White";
}


