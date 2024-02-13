function UploadComplete(sender, args) {
    if (parseInt(args.get_length()) < 4194304) {
        var tokens = args.get_fileName().match(/(.*?)\.(doc|docx|pdf|xls|xlsx)$/);

        var fileName = $get("ContentPlaceHolder1_hfFileLoad").value;
        $get("ContentPlaceHolder1_hfFileLoad").value = fileName;

        var FileExtension = $get("ContentPlaceHolder1_hdnlblFileExt").value;
        $get("ContentPlaceHolder1_hdnlblFileExt").value = FileExtension;

        var FileNameOnly = $get("ContentPlaceHolder1_hdnNameOnly").value;
        $get("ContentPlaceHolder1_hdnNameOnly").value = FileNameOnly;


        $get("ContentPlaceHolder1_lblUploadMsg").innerHTML = "<span style='color:Green;'>File was uploaded successfully</span>";
        $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").value = "";
        $get("ContentPlaceHolder1_pnlFile").style.display = "";
        $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.display = "none";

    }
    else {
        $get("ContentPlaceHolder1_hfFileLoad").innerHTML = "<span style='color:Red;'>File Size Should be less than 4MB</span>";
        $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.backgroundColor = "Red";
        return;
    }
}

function UploadError(sender, args) {
    $get("ContentPlaceHolder1_lblUploadMsg").innerHTML = "<span style='color:Red;'>The only file types allowed are (doc, docx, pdf, xls, xlsx)</span>";
    $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.backgroundColor = "Red";
}

function StartUpload(sender, args) {
    $get("ContentPlaceHolder1_lblUploadMsg").innerText = "";
    //$get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.display = "none";
    $get("ctl00_ContentPlaceHolder1_fuAttachment_ctl02").style.backgroundColor = "White";
}